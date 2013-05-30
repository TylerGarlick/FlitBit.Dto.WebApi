FlitBit.IoC.WebApi
================================
This package configures Microsoft's Web Api framework with FlitBit.IoC.


Instructions for Installation
--------------------------------------------------------------------------------
The Controller Factory will automatically register in the App_Start -> FlitBitConfig.cs

Installation for ContainerPerWebRequest
--------------------------------------------------------------------------------
Register the HttpModule, which enables the ContainerPerWebRequest in FlitBit.IoC.  

In the Web.Config register the following:
<system.web>
	<httpModules>
		<add name="ContainerPerWebRequestHttpModule" type="FlitBit.IoC.WebApi.HttpModules.ContainerPerWebRequestHttpModule, FlitBit.IoC.WebApi" />
	</httpModules>
</system.web>

<system.webServer>
	<modules runAllManagedModulesForAllRequests="true">
		<add name="ContainerPerWebRequestHttpModule" type="FlitBit.IoC.WebApi.HttpModules.ContainerPerWebRequestHttpModule, FlitBit.IoC.WebApi" />
	</modules>
</system.webServer>


--------------------------------------------------------------------------------
1.0.0 Release Notes
--------------------------------------------------------------------------------
Removed all the fluff around the WebApi package, and made the the FlitBit.IoC.WebApi more specific focusing on the specific needs of registering 
FlitBit.IoC with Web Api

