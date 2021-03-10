using AutoMapper;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Resources;
using System;
using System.Globalization;

namespace Nordic.Taxes.Mapping
{
	public class ResourceToModelProfile : Profile
	{
		public ResourceToModelProfile()
		{
			CreateMap<SaveMunicipalityResource, Municipality>();
			CreateMap<SaveYearlyTaxResource, Tax>()
				.ForMember(src => src.TaxType,
						   opt => opt.MapFrom(src => TaxPeriod.Year))
				.ForMember(src => src.From,
						   opt => opt.MapFrom(src => new DateTime(src.Year, 1, 1)))
				.ForMember(src => src.To,
						   opt => opt.MapFrom(src => (new DateTime(src.Year, 1, 1)).AddYears(1).AddTicks(-1)))
				.ReverseMap()
				.ForMember(x => x.Year, x => x.Ignore());

			CreateMap<SaveMonthlyTaxResource, Tax>()
				.ForMember(src => src.TaxType,
						   opt => opt.MapFrom(src => TaxPeriod.Month))
				.ForMember(src => src.From,
						   opt => opt.MapFrom(src => GetMonthDay(src.Year, src.Month, false)))
				.ForMember(src => src.To,
						   opt => opt.MapFrom(src => GetMonthDay(src.Year, src.Month, true)))
				.ReverseMap()
				.ForMember(x => x.Year, x => x.Ignore())
				.ForMember(x => x.Month, x => x.Ignore());

			CreateMap<SaveWeeklyTaxResource, Tax>()
				.ForMember(src => src.TaxType,
						   opt => opt.MapFrom(src => TaxPeriod.Week))
				.ForMember(src => src.From,
						   opt => opt.MapFrom(src => GetDateFromWeekNumberk(src.Year, src.WeekNumber, false)))
				.ForMember(src => src.To,
						   opt => opt.MapFrom(src => GetDateFromWeekNumberk(src.Year, src.WeekNumber, true)))
				.ReverseMap()
				.ForMember(x => x.Year, x => x.Ignore())
				.ForMember(x => x.WeekNumber, x => x.Ignore());


			CreateMap<SaveDailyTaxResource, Tax>()
				.ForMember(src => src.TaxType,
						   opt => opt.MapFrom(src => TaxPeriod.Year))
				.ForMember(src => src.From,
						   opt => opt.MapFrom(src => src.Day))
				.ForMember(src => src.To,
						   opt => opt.MapFrom(src => src.Day))
				.ReverseMap()
				.ForMember(x => x.Day, x => x.Ignore());

		}

		private static DateTime GetMonthDay(int year, int month, bool retLastDay)
		{
			var firstDay = new DateTime(year, month, 1);
			if (retLastDay)
			{
				return firstDay.AddMonths(1).AddDays(-1);
			}
			else
				return firstDay;
		}
		private static DateTime GetDateFromWeekNumberk(int year, int weekNumber, bool retLastDay)
		{

			var dayOfWeek = retLastDay ? 7 : 1;

			DateTime jan1 = new DateTime(year, 1, 1);
			var daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;

			DateTime firstMonday = jan1.AddDays(daysOffset);

			var cal = CultureInfo.CurrentCulture.Calendar;
			//CalendarWeekRule cWR = cal.DateTimeFormat.CalendarWeekRule;
			//DayOfWeek firstDOW = cal.DateTimeFormat.FirstDayOfWeek;

			int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday); //firstDOW - not working // Todo: ?? DayOfWeek.Sunday

			var weekNum = weekNumber;
			if (firstWeek <= 1)
			{
				weekNum -= 1;
			}

			var result = firstMonday.AddDays(weekNum * 7 + dayOfWeek - 1);
			return result;
		}
	}
}
