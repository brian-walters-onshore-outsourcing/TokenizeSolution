using System;
using System.Collections.Generic;
using System.Linq;

namespace MyANTLRparser
{
    #region Types

    [Flags] public enum charTypes
    {
        whitespace                  = 1, // done
        letter                      = 2, // done
        digit                       = 4, // done
        underscore                  = 8, // done
        hexdigit                    = 16, // done
        binarydigit                 = 32, // done
        exponent                    = 64, // done
        minus                       = 128, //done
        symbol                      = 256, // done
        operatordigit               = 512, // done
        declarationsyntax           = 1024, // done
        special                     = 2048,  //done
 
        opening                     = 2*4096, // done
        closing                     = 4*4096, // done
        quote                       = 8*4096, // done
        letterordigit               =16*4096, // done
        validfirst4identifier       =32*4096, // done
        validsecond4identifier      =64*4096, // done
        letterordigitor_            =128*4096, // done
        punctuation                 =256*4096, // done
        realnumberpart              =1024*4096, // done
        integerpart                 =2048*4096, //done 
        



    }

    public static class csharpclassifier
    {
        static csharpclassifier()
        {
            Keywords = new HashSet<string>(new string[]
               {
                   "abstract", "as", "base", "bool", "break", "byte", "case",
                   "catch", "char", "checked", "class", "const","continue",
                   "decimal", "default", "delegate","do", "double", "else", "enum",
                   "event", "explicit", "extern", "false", "finally" , "fixed",
                   "float", "for", "foreach","goto", "if", "implicit", "in", "int",
                   "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly",
                   "ref","return","sbyte","sealed","short","sizeof","stackalloc",
                   "static","string","struct","switch","this","throw","true",
                  "try","typeof","uint","ulong","unchecked","unsafe","ushort",
                   "using","virtual","void","volatile","while"
               }
            );
            
            
        }
       public static charTypes IsWhatType(this char c)
        {
            charTypes rv = (charTypes)0;
            switch(c)
            {
                case ('_'): return charTypes.underscore|charTypes.validfirst4identifier|charTypes.validsecond4identifier|charTypes.letterordigitor_;
                case ('1'):
                case ('0'): return charTypes.digit | charTypes.binarydigit | charTypes.hexdigit | charTypes.validsecond4identifier | charTypes.letterordigitor_ | charTypes.realnumberpart | charTypes.integerpart;
                case 'A': case 'a': case 'B': case 'b': case 'C': case 'c':
                    return charTypes.hexdigit | charTypes.letter | charTypes.letterordigit |
                           charTypes.letterordigitor_ | charTypes.validfirst4identifier | charTypes.validsecond4identifier;
                case 'M': case 'm':
                    return charTypes.letter | charTypes.letterordigit |
                            charTypes.letterordigitor_ | charTypes.validfirst4identifier | charTypes.validsecond4identifier | charTypes.realnumberpart ;
                 case 'D': case 'd':case 'F': case 'f':
                    return charTypes.hexdigit | charTypes.letter | charTypes.letterordigit |
                            charTypes.letterordigitor_ | charTypes.validfirst4identifier | charTypes.validsecond4identifier | charTypes.realnumberpart;
                case 'E': case 'e': return charTypes.exponent |
                    charTypes.hexdigit | charTypes.letter | charTypes.letterordigit |
                    charTypes.letterordigitor_ | charTypes.validfirst4identifier |charTypes.validsecond4identifier | charTypes.realnumberpart;
                case '-': return charTypes.minus | charTypes.operatordigit | charTypes.symbol | charTypes.realnumberpart | charTypes.integerpart|charTypes.symbol;
                case '(': case '[': case '{': return charTypes.symbol | charTypes.opening|charTypes.declarationsyntax ;
                case '<': return charTypes.symbol | charTypes.opening | charTypes.declarationsyntax | charTypes.operatordigit;
                case ')': case ']': case '}': return charTypes.symbol | charTypes.closing | charTypes.declarationsyntax;
                case '>': return charTypes.symbol | charTypes.closing | charTypes.declarationsyntax | charTypes.operatordigit;
                case '"': case '\'': return charTypes.symbol | charTypes.quote ;
                 case ';': case '\\': case '@': case '#': case '$': case ',':
                    return charTypes.punctuation | charTypes.symbol | charTypes.special;
                case '.': return charTypes.punctuation | charTypes.symbol | charTypes.operatordigit | charTypes.realnumberpart;
                case 'U': case 'u': case 'L': case 'l':
                    return charTypes.letter | charTypes.letterordigit |
                        charTypes.letterordigitor_ | charTypes.integerpart;
                case ':': return charTypes.punctuation | charTypes.symbol | charTypes.operatordigit | charTypes.declarationsyntax;
                case '?':
                    return charTypes.symbol | charTypes.operatordigit | charTypes.declarationsyntax;
                case '+':
                case '!':
                case '%':
                case '^':
                case '&':
                case '*':
                case '=':
                case '|':
                case '/':
                case '~':
                    return charTypes.symbol | charTypes.operatordigit;
             }
            if (char.IsWhiteSpace(c))  return charTypes.whitespace;
            if (char.IsDigit(c)) return charTypes.digit | charTypes.letterordigit | charTypes.letterordigitor_ | charTypes.validsecond4identifier ;
            if (char.IsLetter(c)) return charTypes.letter | charTypes.letterordigit | charTypes.letterordigitor_ | charTypes.validsecond4identifier | charTypes.validfirst4identifier;


            throw new Exception($"Unrecognized character '{c}' value: {(int)c}");
        }
       public static bool IsAnyOfTheseTypes(this char c, params charTypes[] types)
        {
            charTypes ct = c.IsWhatType();
            foreach (var type in types)
            {
                if (type == (ct & type)) return true;
            }
            return false;
        }

       static  HashSet<string> Keywords ;
        public static bool IsKeyword(this string identifier)
        {
            return Keywords.Contains(identifier);
        }
    }
    public enum elementType
    {
       
        token,
        lineTerminator,
        whiteSpace,
        comment,
        preProcessorDirective
    }
    [Flags] public enum tokenType
    {
    
        literal                     = 1,
        literalstring               = 2,
        literalnumber               = 4,
        name                        = 8,

        literalInteger              = 1 + 4 + 16,
        literalReal                 = 1 + 4 + 32,
        
        literalInterpolatedString   = 1 + 2 + 64,
        literalverbatumString       = 1 + 2 + 128,
        literalChar                 = 1 + 256,
        

        identifier                  = 512,
        keyword                     = 1024,
        OperatorPunctuator          = 2048,
        @operator                   = 2048 + 4096,
        punctuator                  = 2048 + 2*4096,
        balanced                    = 2048 + 4*4096,


    }

    [Flags] public enum operatorType
    {
      
        primary                         =1,
        unary                           =2,
        range                           =3,

        arithmetic                      =4,
          multiplictive                 =4+8,
          additive                      =4+16,
          shift                         =4+32,
        conditional                     =64,
          relational                    =64 + 128,
          equality                      =64 + 256, 
        bitwise                         =512,
          bitwiseAND                    =512 + 1024,
          bitwiseXOR                    =512 + 2048,
          bitwiseOR                     =512 + 4096,
        logical                         = 2*4096,
          logicalAND                    = 2*4096 + 4*4096,
          logicalOR                     = 2*4096 + 8*4096,
          nullCoalescing                = 2*4096 + 16*4096,
        ternaryConditional              = 32*4096,
        assignment                      = 64*4096,
        lambdaDeclaration               = 128*4096,
        balanced                        = 256*4096,
        special                         = 512*4096,
        other                           = 1024*4096,



    }
    public class ParsedElement
    {
        public ParsedElement(string Data, elementType ElementType)
        {
            this.Data = Data;
            this.ElementType = ElementType;
        }
        public string Data { get; }
        public elementType ElementType { get; }
        
    }

    public class ParsedToken : ParsedElement
    {
        public ParsedToken(string Data, tokenType TokenType)
            :base(Data,elementType.token)
        {
            this.TokenType = TokenType;
        }

        public tokenType TokenType { get; }
    }

    public class ParsedOperator : ParsedToken
    {
        public ParsedOperator(string Data, tokenType TokenType, operatorType OperatorType)
              :base(Data, TokenType)
            {
            this.OperatorType = OperatorType;
            }

        public operatorType OperatorType { get; }
    }

    public class StateTransition
    {
 //       public char CharacterBeingParsed { get;  set; }
 //       public int PriorState { get;  set; }
        public string NewState { get; set; }
        public ParsedElement ElementFound { get; set; }
       



    }
    public class State
    {
        internal State() { }

        public string Name { get; internal set; }
        public Parser theParser { get; internal set; }
        public Func<State, char, StateTransition> Execute { get; internal set; }
    }

    #endregion needed types
    public class Parser
    {
        #region constructors

        public Parser (System.IO.StreamReader streamToParse)
        {
            streamBeingParsed = streamToParse;
        }
        
        public Parser (string stringToParse)
        {
            streamBeingParsed = new System.IO.StringReader(stringToParse);
        }

        #endregion

        #region private fields

        bool parsed;
        bool parsing;

        System.IO.TextReader streamBeingParsed;

        System.Text.StringBuilder collectedCharacters = new System.Text.StringBuilder(1000);
        System.Text.StringBuilder unconsumedCharacters = new System.Text.StringBuilder(100);

        List<ParsedElement> parsedElements = new List<ParsedElement>();

        List<ParsedToken> parsedTokens = new List<ParsedToken>();

        private Dictionary<string, State> StateMachine = new Dictionary<string, State>();
        private string currentState = "";


        #endregion     
        
        #region private properties


        #endregion

        #region private methods

        void log(string s)
        {
            Log?.Invoke(s);
        }
        bool Next(out char c)
        {
            int i = streamBeingParsed.Read();
            if (-1 == i)
            {
                c = '\0'; return false;
            }
            else
            {
                c = (char)i;
              
                
                return true;
            }
        }

  
   
        private string Transition(char c)
        {
            log($"Parsing '{c}' value:{(int)c,4}");
            State s;
            bool found = StateMachine.TryGetValue(currentState, out s);
            if (found)
            {
                log($"Transition:{s.Name,20}->");
                StateTransition st = s.Execute(s,c);
                log($"{st?.NewState??"NULL",20}\n");
                if (null != st?.ElementFound)
                {
                    log($"Consumed: '{unconsumedCharacters}'\n");
                    collectedCharacters.Append(unconsumedCharacters);
                    if (st.ElementFound is ParsedElement pe)
                    {
                        log($"ParsedElement:{pe.Data}\n");
                        parsedElements.Add(pe);
                        ElementParsed?.Invoke(pe);
                        
                    }
                    if (st.ElementFound is ParsedToken pt)
                    {
                        log($"ParsedToken:{pt.Data}\n");
                        parsedTokens.Add(pt);
                        TokenParsed?.Invoke(pt);
                    }
                    if (st.ElementFound is ParsedOperator po)
                    {
                        log($"ParsedOperator:{po.Data}\n");
                        OperatorParsed?.Invoke(po);
                    }
                    unconsumedCharacters.Clear();
                }
                else
                {
                    unconsumedCharacters.Append(c);
                }
                return st?.NewState??"";
            }
            else
            {
                throw new Exception($"State '{currentState}' not found in stateMachine");
            }
        }

        void privateParse()
        {
            char c;
            while(Next(out c))
            {
                currentState = Transition(c);
            }
        }
        #endregion



 
        #region public properties

        public System.IO.TextReader StreamBeingParsed { get { return streamBeingParsed; } }

        public bool IsParsed { get { return parsed; } }

        public  IReadOnlyList<ParsedElement> ParsedElements
        {
            get
            {
                return parsedElements;
            }
            
        }

        public  IReadOnlyList<ParsedToken> ParsedTokens
        {
            get
            {
                return parsedTokens;
            }
        }

        public string UnconsumedData { get { return unconsumedCharacters.ToString(); } }

        public Action<string> Log { get; set; }

        #endregion public properties
 
        #region public events

        public event Action<ParsedElement> ElementParsed;
        public event Action<ParsedToken> TokenParsed;
        public event Action<ParsedOperator> OperatorParsed;

        #endregion

        #region public methods

        public bool ParseAll()
        {
            lock (this)
            {
                if (!parsing)
                {
                    parsed = true;
                    parsing = true;
                    privateParse();
                    return true;
                }
                else
                {
                    return false; ;
                }
            }

        }

        public char ParseNext()
        {
            lock (this)
            {
                if (parsed)
                {
                    return '\0';
                }
                if (!parsing)
                {
                    parsing = true;
                    
                }
                
            }
            char c;
            if ( Next(out c))
            {
                currentState = Transition(c);
                return c;
            }
            return '\0';
            
        }

        public State CreateState(string StateName,  Func<State, char, StateTransition> ExecuteFunction)
        {
            return new State()
            {
                Name    = StateName,
                Execute = ExecuteFunction,
                theParser = this ,                
            };
        }

        public bool Peek(out char c)
        {
            int i = streamBeingParsed.Peek();
            if (-1 == 1)
            {
                c = '\0'; return false;
            }
            else
            {
                c = (char)i; return true;
            }
        }

        public bool AddState(State newState, bool throwException=true)
        {
            if (StateMachine.ContainsKey(newState.Name))
            {
                if (throwException)
                {
                throw new Exception($"StateMachine already has the state '{newState.Name}'");
                }
                else
                {
                    return false;
                }
            }
            StateMachine.Add(newState.Name, newState);
            return true;

        }

        public void AddStates(params State[] states)
        {
            foreach (State s in states)
            {
                AddState(s);
            }
        }

        public bool InitializeStates(IEnumerable<State> states)
        {
            StateMachine = states.ToDictionary(k => k.Name);
            return true;
        }

        #endregion methods

   


    }
}
