using System;
using System.Collections.Generic;
using System.Text;

namespace MyANTLRparser
{
    public static class csharpstatemachineinitializer
    {
        static public void InitToCSharpStatemachine(this Parser p)
        {

            Func<State,char,StateTransition> invalid = (s, c) =>
            {
                return
                  new StateTransition()
                  {
                      NewState = "",
                      ElementFound = new ParsedToken(p.UnconsumedData  , tokenType.invalidToken)
                  };
            };
            


            p.AddStates(p.CreateState("", (s, c) =>
            {
                if (char.IsWhiteSpace(c))
                {
                    return new StateTransition() { NewState = "whitespace" };
                }
                if ('-' == c)
                {
                    return new StateTransition() { NewState = "minus" };
                }
                if ('+' == c)
                {
                    return new StateTransition() { NewState = "plus" };
                }

                if ('_' == c)
                {
                    return new StateTransition() { NewState = "identifier" };
                }

                if ('0' == c)
                {
                    return new StateTransition() { NewState = "zero" };
                }
                if ('\\' == c)
                {
                    return new StateTransition() { NewState = "slash" };
                }
                if ('.' == c)
                {
                    return new StateTransition() { NewState = "dotatstart" };
                }
                if (char.IsDigit(c))
                {
                    return new StateTransition() { NewState = "digitonly" };
                }
                if (char.IsLetter(c))
                {
                    return new StateTransition() { NewState = "letteronly" };
                }
                if (c.IsAnyOfTheseTypes(charType.symbol))
                {
                    return new StateTransition() { NewState = "symbol" };
                }
                if ('\uffff' == c)
                {
                    return new StateTransition() { NewState = "terminalState" };
                }


                throw new Exception($"state ''  does not recognize '{c}' value {(int)c}");

            })
           , p.CreateState("whitespace", (s, c) =>
           {
               if (char.IsWhiteSpace(c))
               {
                   return new StateTransition() { NewState = "whitespace" };
               }
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedElement(p.UnconsumedData, elementType.whiteSpace)
               };
           }) // -> whitespace :whitespace
           , p.CreateState("letteronly", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.letter))
               {
                   return new StateTransition() { NewState = "letteronly" };
               }
               if (c.IsAnyOfTheseTypes(charType.letterordigitor_))
               {
                   return new StateTransition() { NewState = "identifier" };
               }
               if (p.UnconsumedData.IsKeyword())
                   return new StateTransition()
                   {
                       NewState = "",
                       ElementFound = new ParsedToken(p.UnconsumedData, tokenType.keyword)
                   };
               else return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.identifier)
               };

           }) // -> letteronly :keyword :identifier
           , p.CreateState("identifier", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.letterordigitor_))
               {
                   return new StateTransition() { NewState = "identifier" };
               }

               else return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.identifier)
               };

           }) // -> identifier :identifier
           , p.CreateState("digitonly", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.digit))
               {
                   return new StateTransition() { NewState = "digitonly" };
               }
               if ('_' == c)  // digit seperator for numbers is now recognized
               {
                   return new StateTransition() { NewState = "digitonly" };
               }
               if (('L' == c) || ('l' == c))
               {
                   return new StateTransition() { NewState = "longinteger" };
               }
               if (('U' == c) || ('u' == c))
               {
                   return new StateTransition() { NewState = "unsignedinteger" };
               }
               if (('E' == c) || ('e' == c))
               {
                   return new StateTransition() { NewState = "realexp" };
               }
               if ('.' == c)
               {
                   return new StateTransition() { NewState = "realmantissareq" };
               }
               else return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalInteger)
               };

           }) // digitonly longinteger Uint LONGint realexp realmantissareq
           , p.CreateState("zero", (s, c) =>
           {
               if (('x' == c) || ('X' == c))
               {
                   return new StateTransition() { NewState = "hex" };
               }
               if (('b' == c) || ('B' == c))
               {
                   return new StateTransition() { NewState = "hex" };
               }
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedElement(p.UnconsumedData, elementType.whiteSpace)
               };
           })  // -> hex
           , p.CreateState("hex", (s, c) =>
           {

               if (c.IsAnyOfTheseTypes(charType.hexdigit))
               {
                   return new StateTransition() { NewState = "hex" };
               }
               if ('_' == c)  // digit seperator for numbers is now recognized
               {
                   return new StateTransition() { NewState = "hex" };
               }
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalIntegerHex)
               };
           })   // ->hex :literalinthex
           , p.CreateState("binary", (s, c) =>
           {

               if (c.IsAnyOfTheseTypes(charType.binarydigit))
               {
                   return new StateTransition() { NewState = "binary" };
               }
               if ('_' == c)  // digit seperator for numbers is now recognized
               {
                   return new StateTransition() { NewState = "binary" };
               }
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalIntegerBinary)
               };
           }) // -> binary :literalintbin
           , p.CreateState("slash", (s, c) =>
           {
               if (('\\' == c))
               {
                   return new StateTransition() { NewState = "eolcomment" };
               }
               if ('*' == c)
               {
                   return new StateTransition() { NewState = "asteriskcomment" };
               }
               if ('u' == c)  return new StateTransition()
               {
                   NewState = "slashunicode"

               };
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.invalidToken)

               };
           })  // -> eolcomment *comment \quoted
           , p.CreateState("longinteger", (s, c) =>
           {
               if (('U' == c) || ('u' == c))
               {
                   return new StateTransition() { NewState = "unsignedlonginteger" };
               }

               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalInteger)
               };
           })// -> ULONGinteger :literalinteger
           , p.CreateState("unsignedinteger", (s, c) =>
           {
               if (('L' == c) || ('l' == c))
               {
                   return new StateTransition() { NewState = "unsignedlonginteger" };
               }

               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalInteger)
               };
           })// unsignedint : literalinteger
           , p.CreateState("unsignedlonginteger", (s, c) =>
           {

               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalInteger)
               };
           })// :literalinteger
           , p.CreateState("minus", (s, c) =>
           {
               if (char.IsDigit(c))
               {
                   return new StateTransition()
                   {
                       NewState = "digitonly"
                   };
               }
               if ('-' == c)
               {

                   return new StateTransition()
                   {
                       NewState = "incrementdecrement",

                   };
               }
               if ('=' == c)
                   return new StateTransition()
                   {
                       NewState = "opequals",

                   };
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedOperator(p.UnconsumedData, tokenType.@operator, operatorType.additive)
               };
           }) // digitonly negincrementdecrement opequals :operator.additive
           , p.CreateState("incrementdecrement", (s, c) =>
           {

               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedOperator(p.UnconsumedData, tokenType.@operator, operatorType.primary)
               };
           }) // :operator.primary
           , p.CreateState("opequals", (s, c) =>
           {
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedOperator(p.UnconsumedData, tokenType.@operator, operatorType.assignment)
               };
           })  // :operator.assignment
           , p.CreateState("plus", (s, c) =>
           {
               {
                   if (char.IsDigit(c))
                   {
                       return new StateTransition()
                       {
                           NewState = "digitonly"
                       };
                   }
                   if ('+' == c)
                   {

                       return new StateTransition()
                       {
                           NewState = "incrementdecrement",

                       };
                   }
                   if ('=' == c)
                       return new StateTransition()
                       {
                           NewState = "opequals",

                       };
                   return new StateTransition()
                   {
                       NewState = "",
                       ElementFound = new ParsedOperator(p.UnconsumedData, tokenType.@operator, operatorType.additive)
                   };
               }

           }) //digitonly negincrementdecrement opequals :operator.additive
           , p.CreateState("realdigitexpreq", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.digit))
               {
                   return new StateTransition() { NewState = "realdigitexpopt" };
               }


               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.invalidToken)
               };
           })
           , p.CreateState("realdigitexpopt", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.digit))
               {
                   return new StateTransition() { NewState = "realdigitexpopt" };
               }


               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalReal)
               };
           })
           , p.CreateState("realexp", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.digit))
               {
                   return new StateTransition() { NewState = "realdigitexpreq" };
               }
               if (('-' == c) || ('+' == c))
               {
                   return new StateTransition() { NewState = "realdigitexpreq" };
               }

               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.invalidToken)
               };
           }) // -> realdigitexp
           , p.CreateState("realmantissareq", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.digit))
               {
                   return new StateTransition() { NewState = "realmantissaopt" };
               }
               if (('E' == c) || ('e' == c))
               {
                   return new StateTransition() { NewState = "realdigitexpreq" };
               }

               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedToken(p.UnconsumedData, tokenType.invalidToken)
               };
           })// -> realmantissaopt realdigitexpreq :invalidtoken
           , p.CreateState("realmantissaopt", (s, c) =>
             {
                 if (c.IsAnyOfTheseTypes(charType.digit))
                 {
                     return new StateTransition() { NewState = "realmantissaopt" };
                 }
                 if (('E' == c) || ('e' == c))
                 {
                     return new StateTransition() { NewState = "realdigitexpreq" };
                 }

                 return new StateTransition()
                 {
                     NewState = "",
                     ElementFound = new ParsedToken(p.UnconsumedData, tokenType.literalReal)
                 };
             })  // ->realmantissaopt  realdigitexpreq  :literalReal
           , p.CreateState("dotatstart", (s, c) =>
           {
               if (c.IsAnyOfTheseTypes(charType.digit))
               {
                   return new StateTransition() { NewState = "realmantissaopt" };
               }
               return new StateTransition()
               {
                   NewState = "",
                   ElementFound = new ParsedOperator(p.UnconsumedData, tokenType.@operator, operatorType.primary)
               };

           }) // -> realmantissaopt :operator.primary
           , p.CreateState("eolcomment", (s, c) => invalid(s,c))
           , p.CreateState("asteriskcomment", (s, c) => invalid(s,c))
           , p.CreateState("slashquoted", (s, c) => invalid(s,c))
           , p.CreateState("symbol", (s, c) => invalid(s,c))
        ); ;
        }
    }
}
