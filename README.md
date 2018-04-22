# parser
Parser for Simple Actor Language
To be used with ARnActorModel

from ==>
                Actor bla
                    Behavior(""bla"")
                    {
                        Send Console (""bla"")
                    }
                
                 
                 
to ==>

    public class bla : BaseActor
    {
        public bla()
        {
            AddBehavior(new Behavior(
                Pattern = () =>
                {
                    "bla";
                }
                ,
                Apply = () =>
                {
                    SendMessage(Console, "bla");
                }
            ));
        }
    }
