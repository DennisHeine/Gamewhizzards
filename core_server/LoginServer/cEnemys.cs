using Data.Characters.Stats;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LoginServer
{
    class cEnemys
    {                
        public Data.cPositionDataEnemys spawnSingleEnemy(float x1, float z1, float x2, float z2, Data.Characters.EnemyCharacter characterProto, String prevSessID)
        {
            
            Data.cPositionDataEnemys e = new Data.cPositionDataEnemys();
            do
            {
                e.SessionID = genSessionID();
            } while (prevSessID == e.SessionID);

            

            Stats stats = new Stats();
            stats.Health = 100;

            Data.Characters.EnemyCharacter c = new Data.Characters.EnemyCharacter();
            c.protoAssetName = characterProto.protoAssetName;
            c.CharacterStats = stats;
            e.charData = c;

            e.lastUpdate = 0;
            e.moving = false;
            e.rw = 0;
            e.rx = 0;
            e.ry = 0;
            e.y = (float)70;
            e.upperLefX = x1;
            e.upperLefY = z1;
            e.lowerRightX = x2;
            e.lowerRightY = z2;
            float[] ret1 = genRndPoint(x1, z1, x2, z2);
            e.x = ret1[0];
            e.z = ret1[1];
            return e;
        }

        public ConcurrentDictionary<String, Data.cPositionDataEnemys> spawnEnemys(int number, float x1, float z1, float x2, float z2, Data.Characters.EnemyCharacter characterProto)
        {
            ConcurrentDictionary<String, Data.cPositionDataEnemys> ret = new ConcurrentDictionary<String, Data.cPositionDataEnemys>();
            
            String prevSessID = "";
            for (int i = 0; i < number; i++)
            {

                Data.cPositionDataEnemys e= spawnSingleEnemy(x1, z1, x2, z2, characterProto,prevSessID);
                prevSessID = e.SessionID;
                /*
                stats = new Dictionary<StatType, Stat>();
                
                    stats.Add(StatType.STAT_STRENGTH, new Stat(StatType.STAT_STRENGTH));
                    stats[StatType.STAT_STRENGTH].Value = 10;

                e.charData.equipment = new Data.Characters.Equipment.cEquipment();
                Data.Items.Item it = new Data.Items.Item();
                it.ItemStats = stats;
                it.Type = Data.Items.Item.ItemType.ITEM_WEAPON;                
                //e.charData.equipment.Items.Add(Data.Characters.Equipment.cEquipment.StatType.EQU_LEFT_HAND, it);                
                */

                while (!ret.TryAdd(e.SessionID, e))
                    System.Threading.Thread.Sleep(100);
            }
            return ret;
        }

        public Data.cPositionDataEnemys TargetReached(Data.cPositionDataEnemys enemy)
        {
            float x1 = enemy.upperLefX;
            float y1 = enemy.upperLefY;
            float[] ret= genRndPoint( x1, y1, enemy.lowerRightX, enemy.lowerRightY);
            enemy.x = ret[0];
            enemy.y = ret[1];
            return enemy;
        }
        
        private float[] genRndPoint(float x1, float y1, float x2, float y2)
        {
            double range = (double)x2 - (double)x1;
            double sample = (new Random()).NextDouble();
            double scaled = (sample * range) + x1;
            float x = (float)scaled;

            range = (double)y2 - (double)y1;
            sample = (new Random()).NextDouble();
            scaled = (sample * range) + y1;
            float y = (float)scaled;
            float []ret = new float[2];
            ret[0] = x;
            ret[1] = y;
            return ret;

        }

        private String genSessionID()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[25];
            var random = new System.Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString.ToString();
        }

    }
}
