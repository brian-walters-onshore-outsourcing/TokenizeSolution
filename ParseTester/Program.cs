using System;
using MyANTLRparser;

namespace ParseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser(" .\t.a.b.c.1.2.3.0.\\.@.#.$");
            var entry = new StateTransition() { NewState = "" };
            p.Log = Console.Write;
            p.AddStates(p.CreateState("", (s, c) =>
            {
                if (char.IsWhiteSpace(c))
                {
                    return new StateTransition() { NewState = "whitespace" };
                }
 
                else if ('_'==c)
                {
                    return new StateTransition() { NewState = "identifier" };
                }
               
                else if ('0' == c)
                {
                    return new StateTransition() { NewState = "zero" };
                }
                else if ('\\' == c)
                {
                    return new StateTransition() { NewState = "slash" };
                }
                else if (char.IsDigit(c))
                {
                    return new StateTransition() { NewState = "digitonly" };
                }
                else if (char.IsLetter(c))
                {
                    return new StateTransition() { NewState = "letteronly" };
                }
                else if (c.IsAnyOfTheseTypes(charTypes.symbol ))
                {
                    return new StateTransition() { NewState = "symbol" };
                }

                throw new Exception($"state ''  does not recognize '{c}' value {(int)c}");

            })
             ,p.CreateState("whitespace", (s, c) => entry)
             ,p.CreateState("letteronly", (s, c) => entry)
             ,p.CreateState("identifier", (s, c) => entry)
             ,p.CreateState("digitonly", (s, c) => entry)
             ,p.CreateState("zero", (s, c) => entry)
             ,p.CreateState("slash", (s, c) => entry)
             ,p.CreateState("symbol", (s, c) => entry)

            );

            p.ParseAll();
        }
    }
}

