﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BaseLibrary;
using BaseLibrary.equip;
using BaseLibrary.protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace BattlefieldLibrary.config {

    public static class ServerConfig {
        public static string[] SUPPORTED_PROTOCOLS { get; private set; }
        public static readonly ProtocolFactory PROTOCOL_FACTORY = new ProtocolFactory();

        public static Motor[] MOTORS { get; private set; }
        public static Armor[] ARMORS { get; private set; }
        public static Gun[] GUNS { get; private set; }
        public static RepairTool[] REPAIR_TOOLS { get; private set; }
        public static MineGun[] MINE_GUNS { get; private set; }

        public static readonly int MAX_TURN = 5000;

        static ServerConfig() {
            int index = 0;

            KeyValuePair<string, AProtocol>[] supportedProtocols = getSupportedProcotols();
            SUPPORTED_PROTOCOLS = new string[supportedProtocols.Length];
            foreach (var pair in supportedProtocols) {
                SUPPORTED_PROTOCOLS[index++] = pair.Key;
                PROTOCOL_FACTORY.RegisterProtocol(pair.Key, pair.Value);
            }           
        }

        private static KeyValuePair<string, AProtocol>[] getSupportedProcotols() {
            List<KeyValuePair<string, AProtocol>> supportedProtocols = new List<KeyValuePair<string, AProtocol>>();
            Type[] protocolTypes = (Type[]) Assembly.GetAssembly(typeof(AProtocol))
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    typeof(AProtocol).IsAssignableFrom(t) &&
                    t.GetCustomAttribute(typeof(ProtocolDescription)) != null)
                .ToArray();
            foreach (Type protocolType in protocolTypes) {
                AProtocol protocol = (AProtocol) Activator.CreateInstance(protocolType);
                string name =
                    ((ProtocolDescription) protocolType.GetCustomAttribute(typeof(ProtocolDescription))).NAME;
                KeyValuePair<string, AProtocol> keyValuePair = new KeyValuePair<string, AProtocol>(name, protocol);
                supportedProtocols.Add(keyValuePair);
            }
            return supportedProtocols.ToArray();
        }

        public static void SetEquipmentFromFile(string filepath) {
            JObject jObject = JObject.Load(new JsonTextReader(File.OpenText(filepath)));
            MOTORS = jObject["motors"].Select(json => JsonConvert.DeserializeObject<Motor>(json.ToString())).ToArray();
            ARMORS = jObject["armors"].Select(json => JsonConvert.DeserializeObject<Armor>(json.ToString())).ToArray();
            GUNS = jObject["guns"].Select(json => JsonConvert.DeserializeObject<Gun>(json.ToString())).ToArray();
            REPAIR_TOOLS = jObject["repairTools"].Select(json => JsonConvert.DeserializeObject<RepairTool>(json.ToString())).ToArray();
            MINE_GUNS = jObject["mineGuns"].Select(json => JsonConvert.DeserializeObject<MineGun>(json.ToString())).ToArray();
        }

        public static void SetDefaultEquipment() {
            MOTORS = new Motor[] {
                new Motor(MAX_SPEED: 4, ROTATE_IN: 50,ACCELERATION: 10, DECELERATION: 5, MAX_INITIAL_POWER: 30, COST: 0, ID: 1),
                new Motor(5, 55, 12, 6, 40, 50, 2),
                new Motor(6, 60, 13, 5, 50, 100, 3)
            };

            GUNS = new Gun[] {
                new Gun(ID: 1, COST: 0, MAX_BULLETS: 2, MAX_RANGE: 700, SHOT_SPEED: 15,
                    ZONES: new Zone[] {new Zone(5, 20), new Zone(10, 10), new Zone(20, 5), new Zone(30, 3)}),
                new Gun(2, 80, 2, 800, 24,
                    new Zone[] {new Zone(5, 20), new Zone(10, 10), new Zone(20, 5), new Zone(30, 3)}),
                new Gun(3, 150, 2, 500, 16,
                    new Zone[] {
                        new Zone(5, 25), new Zone(10, 15), new Zone(20, 10), new Zone(30, 5), new Zone(40, 3)
                    }),
                new Gun(4, 200, 8, 500, 16,
                    new Zone[] {new Zone(5, 20), new Zone(10, 10), new Zone(20, 5), new Zone(30, 3)})
            };

            ARMORS = new Armor[] {
                new Armor(ID: 1, COST: 0, MAX_HP: 100),
                new Armor(2, 25, 150),
                new Armor(3, 50, 200)
            };

            REPAIR_TOOLS = new RepairTool[] {
                new RepairTool(ID: 1, COST: 0, MAX_USAGES: 5, ZONES: new Zone[] {
                    new Zone(5, 100), new Zone(10, 50), new Zone(20, 25), new Zone(30, 15)
                }),
                new RepairTool(2, 60, 10,
                    new Zone[] {new Zone(5, 100), new Zone(10, 50), new Zone(20, 25), new Zone(30, 15)}),
                new RepairTool(3, 130, 7,
                    new Zone[] {
                        new Zone(5, 125), new Zone(10, 75), new Zone(20, 50), new Zone(30, 25), new Zone(40, 15)
                    }),
                new RepairTool(4, 180, 15,
                    new Zone[] {new Zone(1, 100), new Zone(5, 50), new Zone(10, 25), new Zone(20, 15)})
            };

            MINE_GUNS = new MineGun[] {
                new MineGun(ID: 1, COST: 0, MAX_MINES: 4,
                    ZONES: new Zone[] {new Zone(distance: 5, effect: 20), new Zone(10, 10), new Zone(20, 5), new Zone(30, 3)}),
                new MineGun(2, 80, 8,
                    new Zone[] {new Zone(5, 20), new Zone(10, 10), new Zone(20, 5), new Zone(30, 3)}),
                new MineGun(3, 150, 6, new Zone[] {
                        new Zone(5, 25), new Zone(10, 15), new Zone(20, 10), new Zone(30, 5), new Zone(40, 3)
                    }),
                new MineGun(4, 200, 10,
                    new Zone[] {new Zone(5, 20), new Zone(10, 10), new Zone(20, 5), new Zone(30, 3)})
            };

        }
    }
}
