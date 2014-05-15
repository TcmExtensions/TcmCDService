TcmCDService
============

TcmCDService is a windows service which exposes a self-hosted Windows Communication Foundation (WCF) service. This service can be consumed from any .Net based clients to provide access to the Tridion Content Delivery functionality without having to directly integrate with Tridion.

More information can be found on the [TcmCDService wiki](https://github.com/robvanoostenrijk/TcmCDService/wiki/Home)

TcmCacheService
===============

TcmCacheService provides a simple hosting service for configuring and running a ZeroMQ broker.

### Configuration #

TcmCacheService can be installed or uninstalled as a service by using the "-install" or "-remove" parameters on the command prompt.


    <configSections>
		    <section name="TcmCacheService" type="TcmCacheService.Configuration.Config, TcmCacheService"/>
    </configSections>
    <TcmCacheService>		
	    <!-- ZeroMQBroker exposes both a subscription and a submission port, this allows Tridion virtual machines to send and recieve cache events. The topic ensures that only Tridion instances for the same instance (Topic) receive each others messages -->
	    <setting key="subscriptionUri">tcp://*:5556</setting>
	    <setting key="submissionUri">tcp://*:5557</setting>
	    <setting key="topic">Tridion</setting>
   </TcmCacheService>

The ZeroMQBroker establishes a message broker which Tridion instances can connect to in order to exchange cache event messages.
