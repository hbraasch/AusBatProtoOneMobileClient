#define FIRE_CHANGE_EVENT_ENABLED
// #undef FIRE_CHANGE_EVENT_ENABLED

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;




namespace Mobile.Helpers
{
    /// <summary>
    /// Used to debounce button and menu presses by producing Command objects that only fires once, until re-activated.
    /// </summary>
    public class CommandHelper
    {

        private Dictionary<string, Command> activeCommands = new Dictionary<string, Command>();
        private bool IsCommandBusy = false;

        public Command ProduceDebouncedCommand(Action execute, [CallerMemberName] string commandName = null)
        {
            // Reuse command if already created
            if (activeCommands.ContainsKey(commandName)) return activeCommands[commandName];

            // Create a new command that wraps user method inside IsBusy flagging
            var newCommand = new Command(
                (obj) =>
                {
                    if (!IsCommandBusy)
                    {
                        IsCommandBusy = true;
#if FIRE_CHANGE_EVENT_ENABLED
                        activeCommands[commandName].ChangeCanExecute();
#endif

                        try
                        {
                            execute.Invoke();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            IsCommandBusy = false;
#if FIRE_CHANGE_EVENT_ENABLED
                            activeCommands[commandName].ChangeCanExecute();
#endif
                        }

                    }
                    else
                    {
                        return;
                    }
                },
                (exec) =>
                {
                    return !IsCommandBusy;
                });
            activeCommands.Add(commandName, newCommand);
            return newCommand;


        }

        /// <summary>
        /// Works well if execute is async that runs in its own thread.
        /// This one does not work in all cases when execute is sync. Some components do not use their Command's CanExecute function e.g. MenuItem works, Button and GestureRecogniser does not work
        /// This
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public Command ProduceDebouncedCommand<T>(Action<T> execute, [CallerMemberName] string commandName = null)
        {
            // Reuse command if already created
            if (activeCommands.ContainsKey(commandName)) return activeCommands[commandName];

            // Create a new command that wraps user method inside IsBusy flagging
            var newCommand = new Command<T>(
                (obj) =>
                {
                    if (!IsCommandBusy)
                    {
                        IsCommandBusy = true;
#if FIRE_CHANGE_EVENT_ENABLED
                        activeCommands[commandName].ChangeCanExecute();
#endif

                        try
                        {
                            execute.Invoke(obj);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            IsCommandBusy = false;
#if FIRE_CHANGE_EVENT_ENABLED
                            activeCommands[commandName].ChangeCanExecute();
#endif
                        }

                    }
                    else
                    {
                        return;
                    }
                },
                (exec) =>
                {
                    return !IsCommandBusy;
                });
            activeCommands.Add(commandName, newCommand);
            return newCommand;


        }

        public Command ProduceDebouncedCommand<T>(Func<T, Task> execute, [CallerMemberName] string commandName = null)
        {
            // Reuse command if already created
            if (activeCommands.ContainsKey(commandName)) return activeCommands[commandName];

            // Create a new command that wraps user method inside IsBusy flagging
            var newCommand = new Command<T>(
                async (obj) =>
                {
                    if (!IsCommandBusy)
                    {
                        IsCommandBusy = true;
#if FIRE_CHANGE_EVENT_ENABLED
                        activeCommands[commandName].ChangeCanExecute();
#endif
                        try
                        {
                            await execute.Invoke(obj);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            IsCommandBusy = false;
#if FIRE_CHANGE_EVENT_ENABLED
                            activeCommands[commandName].ChangeCanExecute();
#endif
                        }

                    }
                    else
                    {
                        return;
                    }
                },
                (exec) =>
                {
                    return !IsCommandBusy;
                });
            activeCommands.Add(commandName, newCommand);
            return newCommand;


        }

        /// <summary>
        /// Hand over to non UI thread for a short while to allow UI events to fire
        /// Used especially to ensure progressViews to update
        /// </summary>
        /// <returns></returns>
        public static Task DoEvents()
        {
            var tcs = new TaskCompletionSource<object>();
            Task.Factory.StartNew(() => {
                Thread.Sleep(1);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }
    }
}
