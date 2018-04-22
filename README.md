# parser
Parser for Simple Actor Language

from ==>
                @"Actor bla
                    Behavior(""bla"")
                    {
                        Send Console (""bla"")
                    }
                 ";
                 
                 
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
