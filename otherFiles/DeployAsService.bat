REM Publish applications using FolderProfile

sc create Nordic-Taxes binPath= "C:\Users\vaidotas.cibulskas\source\github\Nordic-Taxes\bin\Release\netcoreapp3.1\publish\Nordic.Taxes.exe"

REM - start service Nordic-Taxes
REM to delete: sc.exe delete Nordic-Taxes

REM try http://localhost:5000/api/Municipalities