# Monitoring

WAS installatie:
- You need to "Turn Windows features on and off", and then select "IIS Management Scripts and Tools" under "Internet Information Services"->"Web Management Tools".
- %windir%\inetsrv\ aan PATH omgevingsvariabele toevoegen.
- in cmd> appcmd.exe set site "Default Web Site" -+bindings.[protocol='net.tcp',bindingInformation='9000:*'] 

- voeg onderstaande toe aan <sites> van 'Default Web Site' in %windir%\inetsrv\Config
    <application path="/MonitoringService" enabledProtocols="net.tcp">
        <virtualDirectory path="/MonitoringService" physicalPath="%SystemDrive\inetpub\wwwroot%" />
    </application>

- in cmd> appcmd.exe set app "Default Web Site/MonitoringService" /enabledProtocols:net.tcp

