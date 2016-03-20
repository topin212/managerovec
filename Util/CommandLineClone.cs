/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/20/2016
 * Time: 21:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Managerovec.Util
{
	delegate void funcDeleg(String[] lel);
	/// <summary>
	/// Description of CommandLineClone.
	/// </summary>
	public class CommandLineClone
	{
		private const String defaultStartPath = @"E:\";
        private const String alternateDefaultStartPath = @"C:\";

        private String currentPath = null;
        private String lastPath = null;

        private String command;
        private String[] argumentes;

        private const String simpleCommandParsePattern = @"(?<command>[^\s]{2,})\s*(?<arguments>[^\$]{2,})?";

        private Match matchResult;
        private Dictionary<string, funcDeleg> commands;

        public CommandLineClone() { 
        	currentPath = defaultStartPath;
        	commands = new Dictionary<string, funcDeleg>();
        	
        	//probably can read this from cfg?
        	commands.Add("dir", dir);
        	commands.Add("ls", dir);
        	commands.Add("cd", cd);
        	commands.Add("cls", cls);
        	commands.Add("clear", cls);
        	commands.Add("mkdir", mkdir);
        	commands.Add("del", del);
        	commands.Add("help", help);
        	
        	commands.Add("touch", touch);
        	commands.Add("rm", rm);
        	commands.Add("open", open);
        	
        }
        
        public CommandLineClone(String path) {
            if (!path.Equals(""))
                currentPath = path;
            else currentPath = alternateDefaultStartPath;
            commands = new Dictionary<string, funcDeleg>();
        	
        	//probably can read this from cfg?
        	commands.Add("dir", dir);
        	commands.Add("ls", dir);
        	commands.Add("cd", cd);
        	commands.Add("cls", cls);
        	commands.Add("clear", cls);
        	commands.Add("mkdir", mkdir);
        	commands.Add("del", del);
        	commands.Add("help", help);
        	
        	commands.Add("touch", touch);
        	commands.Add("rm", rm);
        	commands.Add("open", open);
        }

        private int errorCounter;

        private bool FirstArgumentEmpty(String[] argument)
        {
            return argument.Length == 1 && argument[0].Equals("")?true:false;
        }
        
        public int EnterConsoleMode()
        {
            int returnValue = 0;
            while (true)
            {
                Console.Write(currentPath);
                command = Console.ReadLine();
                matchResult = Regex.Match(command, simpleCommandParsePattern);
                
                command = matchResult.Groups["command"].Value;
                argumentes = matchResult.Groups["arguments"].Value.Split(' ');

                
                try {	
                	commands[command](argumentes);
                } catch (KeyNotFoundException exc) {
                	Console.WriteLine("Command \"" + command + "\" is not implemented.");
                } catch (DirectoryNotFoundException exc){
                	Console.WriteLine("Directory " + argumentes[0] + " cannot be found.");
                } 

                if(errorCounter > 10)
                {
                    Console.WriteLine("\n\n\nJust start googling. You will not find any successfull hints here.");
                    Console.WriteLine(@"I'm done, I quit.");
                    Console.ReadKey();
                    break;
                }
            }

            return returnValue;
        }

        public string tryCommand(String command){
	        matchResult = Regex.Match(command, simpleCommandParsePattern);
	        
	        command = matchResult.Groups["command"].Value;
	        argumentes = matchResult.Groups["arguments"].Value.Split(' ');

	        try {	
	        	commands[command](argumentes);
	        	return command + " successful.";
	        } catch (KeyNotFoundException exc) {
	        	return ("Command \"" + command + "\" is not implemented.");
	        } catch (DirectoryNotFoundException exc){
	        	return ("Directory " + argumentes[0] + " cannot be found.");
	        } 
	        
        }
        

        //===========================================================================================
        //This block should be moved to another class, for the purposes of flexebility and modularity

        private void open(String[] arguments){
        	if(!FirstArgumentEmpty(arguments)){
			   System.Diagnostics.Process.Start(currentPath + arguments[0]);
			}
        }
        private void touch(String[] arguments){
			if(!FirstArgumentEmpty(arguments)){
				var handle = File.Create(currentPath + arguments[0]);
				handle.Close();
			}
        }
        private void rm(String[] arguments){
        	if(!FirstArgumentEmpty(arguments))
        		File.Delete(currentPath + arguments[0]);
        }
        private void dir(String[] arguments)
        {
        	if (FirstArgumentEmpty(arguments))
			{
            	DirectoryInfo dirInfo = new DirectoryInfo(currentPath);
    
	            Console.Write(dirInfo.CreationTime + "\t" + "<" + dirInfo.Extension + ">\t" + dirInfo.Name + "\n");
	
	            DirectoryInfo[] dirs = dirInfo.GetDirectories();
	
	            foreach (var direct in dirs)
	            {
	                Console.Write(direct.CreationTime + "\t" + "<" + direct.Extension + ">\t" + direct.Name + "\n");
	            }
	
	            FileInfo[] files = dirInfo.GetFiles("*.*");
	
	            foreach (var file in files)
	            {
	                Console.Write(file.CreationTime + "\t" + "<" + file.Extension + ">\t" + file.Name + "\n");
	            }
            }
            else { 
                Console.WriteLine("Poor boy, dir doesn't take any arguments! *facepalm*");
                errorCounter++;
            }
        }
        private void cd(String[] arguments)
        {
        	if (!(FirstArgumentEmpty(arguments)))
            {
                if (arguments[0].Equals(".."))
	                currentPath = lastPath;
	            else
	            {
	            	if(Directory.Exists(currentPath + arguments[0] + '\\')){
	            		lastPath = currentPath;
	                	currentPath += arguments[0] + '\\';
	            	}
	            	else
	            		Console.WriteLine("There is no folder named " + arguments[0]);
                }
            }
            else { 
                Console.WriteLine("C'mon, stop missing arguments!");
                errorCounter++;
            }
        }
        private void cls(String[] arguments) {
			if (FirstArgumentEmpty(arguments))
			    Console.Clear();
			else {
			    Console.WriteLine("Again, extra arguments. WHAT WILL I DO WITH THEM?");
			    errorCounter++;
			}
        }
        private void del(String[] arguments) {
        	if (!FirstArgumentEmpty(arguments))
        		Directory.Delete(currentPath + arguments[0]);
        }
        private void mkdir(String[] arguments) {
        	if (!FirstArgumentEmpty(arguments)){
	            DirectoryInfo dir = new DirectoryInfo(currentPath);
	            dir.CreateSubdirectory(arguments[0]);
        	}
        }
        private void help(String[] arguments)
        {
            Console.WriteLine("dir(ls) - wanna know about dirs? (no args)");
            Console.WriteLine("cd - change dir (one argument)");
            Console.WriteLine("del - removes a dir(one argument)");
            Console.WriteLine("help - this little helper(no arguments)");
            Console.WriteLine("bigBrotherWatches - completely secret stuff");
        }
        
	}
}
