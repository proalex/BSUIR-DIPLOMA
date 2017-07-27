using System.Collections.Generic;

namespace Client
{
    public struct Generator
    {
        public uint Number;
        public byte Load;
    }

    public static class DataProxy
    {
        public static List<Generator> Generators
        {
            get
            {
                ActiveGenerators activeGenerators = DataStorage.GetData<ActiveGenerators>();

                if (activeGenerators == null || activeGenerators.Generators == null)
                {
                    return null;
                }
                else
                {
                    List<Generator> result = new List<Generator>();
                    uint i = 1;

                    foreach (var active in activeGenerators.Generators)
                    {
                        result.Add(new Generator()
                        {
                            Number = i,
                            Load = (byte)((double)active.ActiveUsers / active.Users * 100)
                        });

                        i++;
                    }

                    return result;
                }
            }
        }
    }
}
