using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Notifications;
using YueBa.Global;

namespace YueBa.Utils
{
    public class TileService
    {
        public static void CreateTiles(EventItem primaryTile)
        {
            XDocument xDoc = new XDocument(
                new XElement("tile", new XAttribute("version", 3),
                    new XElement("visual",
                        // Small Tile
                        new XElement("binding", new XAttribute("displayName", "TodoList"), new XAttribute("template", "TileSmall"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", primaryTile.img)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", primaryTile.name, new XAttribute("hint-style", "caption")),
                                    new XElement("text", primaryTile.detail, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", primaryTile.startTime.ToString(), new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", primaryTile.endTime.ToString(), new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),

                        // Medium Tile
                        new XElement("binding", new XAttribute("displayName", "TodoList"), new XAttribute("template", "TileMedium"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", primaryTile.img)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", primaryTile.name, new XAttribute("hint-style", "caption")),
                                    new XElement("text", primaryTile.detail, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", primaryTile.startTime.ToString(), new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", primaryTile.endTime.ToString(), new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),

                        //Wide Tile
                        new XElement("binding", new XAttribute("displayName", "TodoList"), new XAttribute("template", "TileWide"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", primaryTile.img)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", primaryTile.name, new XAttribute("hint-style", "caption")),
                                    new XElement("text", primaryTile.detail, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", primaryTile.startTime.ToString(), new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", primaryTile.endTime.ToString(), new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        )
                    )
                )
             );

            Windows.Data.Xml.Dom.XmlDocument xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            TileNotification notification = new TileNotification(xmlDoc);
            updater.Update(notification);
        }

        public static async void UpdateTiles()
        {

            var MyEvents = await Services.EventServices.getAllEventsParticipatesIn(Store.getInstance().getToken());
            foreach (var element in MyEvents.events)
            {
                element.img = Config.api + element.img;
                CreateTiles(element);
            }
            MyEvents = await Services.EventServices.getAllOwnedEvents(Store.getInstance().getToken());
            foreach (var element in MyEvents.events)
            {
                element.img = Config.api + element.img;
                CreateTiles(element);
            }
        }
    }
}
