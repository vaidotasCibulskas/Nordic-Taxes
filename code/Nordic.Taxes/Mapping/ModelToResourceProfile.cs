using AutoMapper;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Extensions;
using Nordic.Taxes.Resources;

namespace Nordic.Taxes.Mapping
{
	public class ModelToResourceProfile : Profile
	{
		public ModelToResourceProfile()
		{
			CreateMap<Municipality, MunicipalityResource>();
			CreateMap<Tax, TaxResource>()
				.ForMember(src => src.TaxType,
						   opt => opt.MapFrom(src => src.TaxType.ToDescriptionString()));
			CreateMap<Tax, SpecificDateTaxResource>();
		}
	}
}
