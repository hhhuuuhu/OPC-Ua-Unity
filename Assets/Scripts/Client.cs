using UnityEngine;
using UnityEngine.UI;
using Opc.Ua.Client;
using Opc.Ua;
using Opc.Ua.Configuration;
using System;
using System.Threading.Tasks;

public class Client
{
    private ApplicationConfiguration config;
    public double dataFromServer;
    public Session session;

    public async void Init()
    {
        config = new ApplicationConfiguration()
        {
            ApplicationName = "Test-Client",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration { ApplicationCertificate = new CertificateIdentifier() },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
        };

        await config.Validate(ApplicationType.Client);
        if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
        {
            config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
        }
    }
    public async Task<ServerNode> Fetchdata(string nodeId)
    {
        Init();
        using (session = await Session.Create(config, new ConfiguredEndpoint(null, new EndpointDescription("opc.tcp://127.0.0.1:4840/")), true, "", 60000, null, null))
        {
            var val = session.ReadValue(NodeId.Parse("ns=2;i=2"));
            double value = Math.Round(Convert.ToDouble(val.ToString()), 3);
            string name = Convert.ToString(session.ReadNode(nodeId));
            ServerNode serverNode = new ServerNode(name, value);
            return serverNode;
            //return referenceDescriptions;
        }
    }
}

public class ServerNode
{
    private string nodename;
    private double nodeValue;

    public ServerNode(string name, double value)
    {
        this.nodename = name;
        this.nodeValue = value;
    }
    public string Nodename
    {
        get { return nodename; }
    }

    public double Nodevalue
    {
        get { return nodeValue; }
    }
}