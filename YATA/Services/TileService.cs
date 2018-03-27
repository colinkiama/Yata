using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using YATA.Model;

namespace YATA.Services
{
    public partial class TileService
    {
        internal static void UpdateLiveTile(ObservableCollection<ToDoTask> AllTasksToDo)
        {
            List<ToDoTask> IncompleteTasks = GetListOfIncompleteTasks(AllTasksToDo);
            TileContent tileContent = new TileContent();
            if (IncompleteTasks.Count > 0)
            {
                tileContent = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileMedium = new TileBinding()
                        {
                            Branding = TileBranding.Logo,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = "To-Do:",
                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = "• " + IncompleteTasks[0].Content,
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(2, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(3, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(4, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(5, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(6, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(7, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(8, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(9, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    }
                }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Branding = TileBranding.Logo,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = "To-Do:",
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                   new AdaptiveText()
                    {
                        Text = "• " + IncompleteTasks[0].Content,
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(2, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(3, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(4, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(5, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(6, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(7, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(8, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(9, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Caption,
                        HintWrap = true
                    }
                }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Branding = TileBranding.Logo,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = "To-Do:",
                        HintStyle = AdaptiveTextStyle.BodySubtle,
                        HintWrap = true,
                        HintMaxLines = 2
                    },
                  new AdaptiveText()
                    {
                        Text = "• " + IncompleteTasks[0].Content,
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(2, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(3, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(4, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(5, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(6, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(7, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(8, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = ReturnTextBasedOnCount(9, IncompleteTasks),
                        HintStyle = AdaptiveTextStyle.Body,
                        HintWrap = true
                    }
                }
                            }
                        }
                    }
                };
            }
            else
            {
                tileContent = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileMedium = new TileBinding()
                        {
                            Branding = TileBranding.Logo,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = "No Tasks To Do",
                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                        HintWrap = true
                    },
                    new AdaptiveText()
                    {
                        Text = "Add new tasks so they show up here!",
                        HintWrap = true
                    }
                }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Branding = TileBranding.Logo,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = "No Tasks To Do",
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText()
                    {
                        Text = "Add new tasks so they show up here!",
                        HintWrap = true
                    }
                }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Branding = TileBranding.Logo,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = "No Tasks To Do",
                        HintStyle = AdaptiveTextStyle.SubtitleSubtle,
                        HintWrap = true,
                        HintMaxLines = 2
                    },
                    new AdaptiveText()
                    {
                        Text = "Add new tasks so they show up here!",
                        HintStyle = AdaptiveTextStyle.Subtitle,
                        HintWrap = true
                    }
                }
                            }
                        }
                    }
                };
            }


            // Create the tile notification
            var tileNotif = new TileNotification(tileContent.GetXml());

            // And send the notification to the primary tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);
        }

        private static BindableString ReturnTextBasedOnCount(int countToCheckFor, List<ToDoTask> IncompleteTasks)
        {
            return IncompleteTasks.Count >= countToCheckFor  ? "• " + IncompleteTasks[countToCheckFor - 1].Content : "";
        }

       

        private static List<ToDoTask> GetListOfIncompleteTasks(ObservableCollection<ToDoTask> allTasksToDo)
        {
            List<ToDoTask> CompletedTasksToReturn = allTasksToDo.Where(p => !p.isCompleted).ToList();
            return CompletedTasksToReturn;
        }
    }
}
