using System.Collections.Generic;
using System.Linq;

namespace ControllerServer
{
    public static class TaskDistributor
    {
        public static void Distribute(GeneratorTask task, int users)
        {
            int currentUsers = 0;

            foreach (var generator in ServerState.Generators)
            {
                if (generator.Tasks.ContainsKey(task))
                {
                    currentUsers += generator.Tasks[task];
                }
            }

            int diff = users - currentUsers;

            if (diff > 0)
            {
                int added = 0;
                int addon = diff/ServerState.Generators.Count;
                int remainder = diff % ServerState.Generators.Count;

                var free = ServerState.Generators.Where(obj => !obj.Tasks.ContainsKey(task))
                    .OrderByDescending(obj => obj.VirtualUsers - obj.CurrentUsers);

                var executing = ServerState.Generators.Where(obj => obj.Tasks.ContainsKey(task))
                    .OrderBy(obj => obj.Tasks[task]);

                var sorted = free.Concat(executing);

                for (int i = 0; i < sorted.Count() && added < diff; i++)
                {
                    var current = sorted.ElementAt(i);

                    if (current.Tasks.ContainsKey(task))
                    {
                        int toAdd = addon + (remainder > 0 ? remainder-- : 0);

                        if (current.UpdateTask(task, current.Tasks[task] + toAdd))
                        {
                            added += toAdd;
                        }
                    }
                    else
                    {
                        int toAdd = addon + (remainder > 0 ? remainder-- : 0);

                        if (current.AddTask(task, toAdd))
                        {
                            added += toAdd;
                        }
                    }
                }
            }
            else if (diff < 0)
            {
                int i = 0;
                List<Generator> generators = new List<Generator>();

                foreach (var generator in ServerState.Generators)
                {
                    if (generator.Tasks.ContainsKey(task))
                    {
                        generators.Add(generator);
                    }
                }

                int[] generatorsUpdatedLoading = new int[generators.Count()];

                for (int j = 0; j < generators.Count; j++)
                {
                    generatorsUpdatedLoading[j] = generators[j].Tasks[task];
                }

                do
                {
                    for (int j = 0; j < generators.Count && i != -diff; j++)
                    {
                        generatorsUpdatedLoading[j]--;
                        i++;
                    }
                } while (i != -diff);

                for (int j = 0; j < generatorsUpdatedLoading.Length; j++)
                {
                    if (generatorsUpdatedLoading[j] < 1)
                    {
                        generators[j].RemoveTask(task);
                    }
                    else
                    {
                        generators[j].UpdateTask(task, generatorsUpdatedLoading[j]);
                    }
                }
            }
        }
    }
}
