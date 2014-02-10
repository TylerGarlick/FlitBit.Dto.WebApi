FlitBit.IoC.WebApi
================================
This package configures Microsoft's Web Api framework with FlitBit.IoC.

Deprecated the module registration in light of using the Dependency Resolver, and utilizing the proper methods to resolve objects and instances..  



FlitBitConfig.cs
--------------------------------
You should have this line of code in the registration class now.

GlobalConfiguration.Configuration.DependencyResolver = new FlitBitWebApiDependencyResolver(Container.Current);


Notice the Container is passed into the DependecyResolver

--------------------------------------------------------------------------------
Release Notes
--------------------------------------------------------------------------------
See https://github.com/TylerGarlick/FlitBit.IoC.WebApi/blob/master/RELEASE-NOTES.md
