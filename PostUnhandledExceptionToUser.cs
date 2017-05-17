using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace Bot_Application1.Exceptions
{
    public class PostUnhandledExceptionToUser : IPostToBot
    {
        private readonly ResourceManager resources;
        private readonly IPostToBot inner;
        private readonly IBotToUser botToUser;
        private readonly TraceListener trace;

        public PostUnhandledExceptionToUser(IPostToBot inner, IBotToUser botToUser, ResourceManager resources, TraceListener trace)
        {
            SetField.NotNull(out this.inner, nameof(inner), inner);
            SetField.NotNull(out this.botToUser, nameof(botToUser), botToUser);
            SetField.NotNull(out this.resources, nameof(resources), resources);
            SetField.NotNull(out this.trace, nameof(trace), trace);
        }

        async Task IPostToBot.PostAsync(IActivity activity, CancellationToken token)
        {
            try
            {
                await this.inner.PostAsync(activity, token);
            }
            catch (Exception error)
            {
                try
                {
                    if (Debugger.IsAttached)
                    {
                        var message = this.botToUser.MakeMessage();
                        message.Text = $"Exception: { error.Message}";
                        message.Attachments = new[]
                        {
                            new Attachment(contentType: MediaTypeNames.Text.Plain, content: error.StackTrace)
                        };

                        await this.botToUser.PostAsync(message);
                    }
                    else
                    {
                        await this.botToUser.PostAsync("My Personal Error Message");
                    }
                }
                catch (Exception inner)
                {
                    this.trace.WriteLine(inner);
                }

                throw;
            }
        }
    }
}