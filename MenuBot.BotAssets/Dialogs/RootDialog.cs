using MenuBot.BotAssets.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBot.BotAssets.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public Task StartAsync(IDialogContext context)
        {
            try
            {
                context.Wait(MessageReceivedAsync);
            }
            catch (OperationCanceledException error)
            {
                return Task.FromCanceled(error.CancellationToken);
            }
            catch (Exception error)
            {
                return Task.FromException(error);
            }

            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            IMessageActivity message = await result;


            switch (message.Text)
            {
                case "Welcome":
                    await this.WelcomeMessageAsync(context);
                    break;
                case "Reset":
                    await this.StartOverAsync(context, "The conversation has been reset.");
                    break;
                case "Compare Support Plans":
                    await context.PostAsync("Go check out https://azure.microsoft.com/en-gb/support/plans/ ");
                    //really here you would call or forward to another dialog
                    //context.Call(new SupportQnADialog(), this.ResumeAfterICTQnADialog);
                    //await context.Forward(new SupportQnADialog(), this.ResumeAfterSupportQnADialog,message, CancellationToken.None);

                    break;

                default:
                    var options = MenuHelpers.getMenuOptions(message.Text);

                    if (options?.Length > 0)
                    {
                        //render the next lot of buttons
                        await this.MenuButtonsAsync(context, $"Thanks for choosing [{message.Text}].", options);                       
                    }
                    else
                    {
                        var errorText = $"I don't have an option for '${message.Text}' yet. We'll add something for this soon. I'm goign to go back to the beginning. ";
                        // do something here with the text typed to the user that didn't match a menu option.
                        await this.StartOverAsync(context, errorText);
                    }
                    break;
            }

        }


        private async Task WelcomeMessageAsync(IDialogContext context)
        {
            await this.MenuButtonsAsync(context, "Welcome to our bot! ", "Home");
        }

        private async Task MenuButtonsAsync(IDialogContext context, string messageText, string menuOptionsText)
        {
            var reply = context.MakeMessage();

            var menuOptions = MenuHelpers.getMenuOptions(menuOptionsText);

            reply.AddHeroCard(messageText, menuOptions);

            await context.PostAsync(reply);

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MenuButtonsAsync(IDialogContext context, string messageText, IEnumerable<string> menuOptions)
        {
            var reply = context.MakeMessage();

            reply.AddHeroCard(messageText, menuOptions);

            await context.PostAsync(reply);

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task StartOverAsync(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            await context.PostAsync(message);
            // reset any fields 
            // this.contactForm = new Models.ContactForm();
            await this.WelcomeMessageAsync(context);
        }

    }

}
