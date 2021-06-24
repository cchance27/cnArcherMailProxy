This project contains an ASP.NET 4.0 HTTPHandler that handles converting a JSON Post 
to an email containing a cnArcher file that can be used to generate a mobile workorder.

The HTTP Handler handles posts to *.cnMail for example http://localhost/send.cnMail

Example JSON expected by this HTTP Handler:
```json
{
	"name": "Someones Name",
	"Date": "2020-04-04",
	"Tech": "SomeTechName",
	"TechEmail": "test@test.com",
	"ESN": "0a-00-3e-44-44-44",
	"Company": "",
	"EIP": "101101",
	"Account": "44444",
	"Phones": "444-3335,5554444,423-4333",
	"Address": "34 Someone Dr
	Simpson Bay",
	"Package": "Something speed 8/2",
	"vlan": "20",
	"Username": "username1", 
	"Password": "password1",
	"Notes": "This is a bunch of information that i think could be helpful.",
	"Firmware": "16.1.1"
}
```

Once your DLL is built and copied to your webapps /bin directory some adjustments are required to EngageIP's web.config to enable the new handler.
```xml
<configuration>
  <appSettings>
	<add key="cnMailServer" value="my.emailserver.com" />
	<add key="cnUsername" value="cnArcher" />
	<add key="cnPassword" value="password" />
	<add key="cnFromAddress" value="noreply@emailserver.com" />
	<add key="cnMailIsSSL" value="false" />
	<add key="cnMailPort" value="587" />
  </appSettings>
  <system.web>
    <httpHandlers>
      <add verb="POST" path="*.cnMail" type="cnArcherMailProxy.cnMail, cnArcherMailProxy" />
    </httpHandlers>
  </system.web>
  <system.webServer>
    <handlers accessPolicy="Read, Script">
      <remove name="cnmail" />
      <add name="cnmail" path="*.cnmail" verb="POST" type="cnArcherMailProxy.cnMail, cnArcherMailProxy" modules="IsapiModule" scriptProcessor="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll" resourceType="Unspecified" requireAccess="None" preCondition="classicMode,runtimeVersionv4.0,bitness64" />
    </handlers>
  </system.webServer>
</configuration>
```
