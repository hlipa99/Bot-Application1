using Autofac;
using Bot_Application1.Exceptions;
using Microsoft.Bot.Builder.Autofac.Base;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using NLP;
using NLP.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Bot_Application1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new DefaultExceptionMessageOverrideModule());

            builder
                .RegisterAdapterChain<IPostToBot>
            (
                typeof(EventLoopDialogTask),
                typeof(SetAmbientThreadCulture),
                typeof(PersistentDialogTask),
                typeof(ExceptionTranslationDialogTask),
                typeof(SerializeByConversation),
                typeof(Exceptions.PostUnhandledExceptionToUser),
                typeof(LogPostToBot)
            )
            .InstancePerLifetimeScope();
      

            builder
                .Register(c => new CachingBotDataStore(c.ResolveKeyed<IBotDataStore<BotData>>(typeof(ConnectorStore)), CachingBotDataStoreConsistencyPolicy.LastWriteWins))
                .As<IBotDataStore<BotData>>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Update(Conversation.Container);

    
      
        }
    }
}
