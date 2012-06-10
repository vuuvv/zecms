using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Responses;
using ElFinder.Connector.Commands;

namespace ElFinder.Connector
{
    public class CommandDispatcher
    {
        protected System.Collections.Specialized.NameValueCollection Parameters { get; set; }

        protected System.Web.HttpFileCollection Files { get; set; }

        protected Dictionary<string, Func<ICommand>> Commands = new Dictionary<string, Func<ICommand>>();

        public CommandDispatcher(System.Web.HttpRequest request)
        {
            this.Parameters = request.QueryString.Count>0 ? request.QueryString : request.Form;
            this.Files = request.Files;
        }

        public Response DispatchCommand()
        {
            var cmdName = Parameters["cmd"];

            if (string.IsNullOrEmpty(cmdName))
            {
                return new ErrorResponse("Command not set");
            }

            ICommand cmd = null;

            switch (cmdName)
            {                
                case "open":
                    if (!string.IsNullOrEmpty(Parameters["init"]) && Parameters["init"] == "true")
                        cmd = new Init();
                    else
                    {
                        cmd = new Open(Parameters["target"]);                        
                    }
                    break;
                case "mkdir":
                    cmd = new MkDir(Parameters["current"], Parameters["name"]);
                    break;
                case "rm":
                    cmd = new Rm(Parameters["current"], Parameters["targets[]"]);
                    break;
                case "rename":
                    cmd = new Rename(Parameters["current"], Parameters["target"], Parameters["name"]);
                    break;
                case "upload":                    
                    cmd = new Upload(Parameters["current"], Files);
                    break;
                case "ping":
                    cmd = new Ping();
                    break;
                case "duplicate":
                    cmd = new Duplicate(Parameters["current"], Parameters["target"]);
                    break;
                case "paste":
                    cmd = new Paste(Parameters["src"], Parameters["dst"], Parameters["targets[]"], Parameters["cut"]);
                    break;
            }

            if (cmd == null)
            {
                return new ErrorResponse("Unknown command");
            }

            return cmd.Execute();

            return new ErrorResponse("Unknown error");
        }
    }
}
