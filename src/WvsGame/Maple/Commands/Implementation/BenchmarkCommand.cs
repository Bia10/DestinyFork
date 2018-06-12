using System;

using Destiny.IO;
using Destiny.Maple.Characters;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class BenchmarkCommand : Command // Todo: rework
    {
        public override string Name
        {
            get
            {
                return "benchmark";
            }
        }

        public override string Parameters
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool IsRestricted
        {
            get
            {
                return true;
            }
        }

        public override void Execute(Character caller, string[] args)
        {
            if (caller == null) return;

            if (args.Length != 0)
            {
                ShowSyntax(caller);
            }

            else
            {
                Character.Notify(caller, "Benchmark Initiated! ");

                Action firstFunction = Item.consumableAction;
                Action secondFunction = Item.consumableActionHashSet;

                int iterations = 10000;

                double resultTimeFunc1 = Tracer.GetExecutionTime(" Benchmark Results for Func1!", iterations, firstFunction);
                double resultTimeFunc2 = Tracer.GetExecutionTime(" Benchmark Results for Func2!", iterations, secondFunction);

                string resultBegin = string.Format("Results on {0} iterations: ", iterations);          
                Character.Notify(caller, resultBegin);

                string resultStrF1 = string.Format("F1 TotalTime elapsed {0} milliseconds!", resultTimeFunc1);
                Character.Notify(caller, resultStrF1);

                string resultStrF2 = string.Format("F2 TotalTime elapsed {0} milliseconds!", resultTimeFunc2);
                Character.Notify(caller, resultStrF2);
            }
        }
    }
}