This project contains an ASP.NET 2.0 HTTPHandler that handles converting a JSON Post 
to an email containing a cnArcher file that can be used to generate a mobile workorder.

The HTTP Handler handles posts to *.cnMail for example http://localhost/send.cnMail

Example JSON expected by this HTTP Handler:
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

Once your DLL is built and copied to your webapps /bin directory some adjustments are required to EngageIP's web.config to enable the new handler.

<appSettings>
   <add key="cnMailServer" value="hostnameForSmtpServer" />
   <add key="cnUsername" value="usernameForSmtp" />
   <add key="cnPassword" value="passwordForSmtp" />
   <add key="cnFromAddress" value="noreply@something.com" />
   <add key="cnMailIsSSL" value="true" />
   <add key="cnMailPort" value="587" />
</appSettings>
<system.web>
  <httpHandlers>
    <add verb="POST" path="*.cnMail" type="cnArcherProxy.cnMail, cnArcherProxy" />
  </httpHandlers>
</system.web>
<system.webServer>
  <handlers>
    <add name="cnArcherProxy" path="*.cnMail" verb="POST" modules="IsapiModule" scriptProcessor="%windir%\Microsoft.NET\Framework64\v2.0.50727\aspnet_isapi.dll" resourceType="Unspecified" requireAccess="Script" preCondition="bitness64" />
  </handlers>
</system.webServer>