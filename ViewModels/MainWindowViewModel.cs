/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/20/2016
 * Time: 19:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Linq;
using Managerovec.Models;
using Managerovec.Util;

namespace Managerovec.ViewModels
{
	/// <summary>
	/// ViewModel for the main window. Will contain some data.
	/// 
	/// It contains List of FileContainers, that represents all the files we have changed, 
	/// or added tag to.
	/// We only need to push a file to the list if we added some tags to it.
	/// which means, that if we pressed the addTagButton, it should transfer here a 
	/// tag parameter, and create an object with that.
	/// 
	/// It also should have a command line imitator, but this is a different story
	/// </summary>
	public class MainWindowViewModel : BaseViewModel 
	{
		//So, we created our models here. 
		//properties moved to line 50

		//Now we need to add a command to add tags.
		//But prior to that we need to show files in FileListView.
		//And prior to that I will need to fix columns in there.
		//columns are done, now we need 
		//Idea - property like this
		//		
		//		File _file;
		//		
		//		public string filename {
		//			get { return _file.name; }	}
		//
		//This will help, I think
		//Gotta make tea
		//TEATIME NEACH
		
		//Also a currentPath property is needed along with previousPath
		//By default currentPath is set to C: drive.
		
		string _selectedFileName;
		
		public string selectedFileName {
			get { return _selectedFileName; }
			set { _selectedFileName = value; 
				RaisePropertyChanged("selectedFileName");}
		}
		
		string _currentPath;
		public string currentPath {
			get { return _currentPath; }
			set { _currentPath = value; 
				RaisePropertyChanged("currentPath");}
		}
		
		string _previousPath;
		public string previousPath {
			get { return _previousPath; }
			set { _previousPath = value; 
				RaisePropertyChanged("previousPath");}
		}
		
		//Some commands here:
		public RelayCommand fileListViewDoubleClickCommand { get; set; }
		public RelayCommand cliProcessorCommand { get; set; }
		public RelayCommand fileListViewSelectionChangedCommand { get; set; }
		public RelayCommand addTagCommand { get; set; }
		//I will need a public List<> property to display files.
		//should I use list<list<file>>? Nope, I think this is a bad idea, and I will figure out a better way
		List<FileContainer> _files;
		public List<FileContainer> files {
			get { return _files; }
			set { _files = value; 
				RaisePropertyChanged("files");}
		}
		
		List<CliCommandContainer> _cliCommandHistory;
		public List<CliCommandContainer> cliCommandHistory {
			get { return _cliCommandHistory; }
			set { _cliCommandHistory = value; 
				RaisePropertyChanged("cliCommandHistory");}
		}
		
		//id of that selected file, which is needed later on, while saving the tags.
		int selectedFile; 
		List<string> _selectedTags;
		
		public List<string> selectedTags {
			get { return _selectedTags; }
			set { _selectedTags = value; 
				RaisePropertyChanged("selectedTags");}
		}
		//so, we have our properties, now we need to fill them up, and then bind them to 
		//both listViews, and we can test that.
		
		//I will need an instance of my old ConsoleClone to help me with dat task!
		CommandLineClone ccv2 = new CommandLineClone(@"C:\");
		
		public MainWindowViewModel()
		{
			#region populating listView with data
			//initialize the Path variable :)
			currentPath = @"C:\";
			//here are the calls for populating lists with data.
			files = new List<FileContainer>();
			cliCommandHistory = new List<CliCommandContainer>();
			getFilesAndDirectories();
			//so, this is done. Now it's time to bring in some commands.
			#endregion
			#region assigning commands to listView.. not done yet
			fileListViewDoubleClickCommand = new RelayCommand(listViewDoubleClick, param=>true);
			#endregion
			//I will handle them later. Now - the fun part:
			//1) cli
			//2) selection
			#region selection from fileListView
			fileListViewSelectionChangedCommand = new RelayCommand(changeInternalSelection);
			#endregion
			#region cli
			//So, the fun part. We need to react on enter............
			//will be checked in the code behind, not that much of mvvm right now.
			//Rememeber: nothing happens when you break the pattern. There is no design pattern police :)
			//But still, how do you bind events in View to Commands in ViewModel if only button-driven
			//events are supported?
			//hehe, a small obfuscation with param=>true :)
			cliProcessorCommand = new RelayCommand(FcliProcessorCommand, param=>true);
			#endregion
			#region addingTags
			addTagCommand = new RelayCommand(addTagToFilecontainer);
			#endregion
		}

		/// <summary>
		/// Gets all files and directories of a current path.
		/// possibly I'll need to accept an object parameter
		/// in order for this to fit in Util.RelayCommand
		/// 
		/// To be clear, I take directory as a file, except for its extension is 'dir'
		/// 
		/// And also, here I will need to check if I already have a saved file, so
		/// TODO check if there already is a saved tag file.
		/// </summary>
		void getFilesAndDirectories(){
			DirectoryInfo currentDir = new DirectoryInfo(currentPath);
			DirectoryInfo[] internalDirs = currentDir.GetDirectories();
			FileInfo[] internalFiles = currentDir.GetFiles();
			
			foreach (var directory in internalDirs) {
				_files.Add(new FileContainer(directory.Name, directory.FullName, "<dir>", directory.Attributes.ToString(), directory.CreationTime));
			}
			//now, we populated that with directories, but the files remain untouched...
			foreach (var file in internalFiles) {
				_files.Add(new FileContainer(file.Name, file.FullName, "<" + file.Extension + ">", file.Attributes.ToString(), file.CreationTime));
			}
			//populated? great. Now we need to take this, and bind it to the fileListView.
		}
		
		//well, I'll handle double-clicks a bit later. 
		void listViewDoubleClick(object obj)
		{
			MessageBox.Show("Hehehehe");
		}
		
		//Okay, Nevermind. This doesn't work yet.
		//So, let's pass to the selection event.
		void FcliProcessorCommand(object obj){
			cliCommandHistory.Add(new CliCommandContainer(obj as string, ccv2.tryCommand(obj as string)));
			RaisePropertyChanged("cliCommandHistory");
		}
		
		//in order to display that guy properly, I need a List<String> property somewhere above.
		//with that in mind, i need to know the name of the object I'm looking for, and that's easy, it
		//is passed as an object.
		//Or was the idea of passing the whole element good?
		//Now we accept an object, that already has it's full name and it's index can be easily found.
		/// <summary>
		/// Reacts to selection change	
		/// </summary>
		/// <param name="selectedItemName">In this case object type is FileContainer.</param>
		void changeInternalSelection(object selectedItemName){
			//Now I need to find an item with that name in my list. Which could be done by Linq
			var ob1 = selectedItemName as FileContainer;
			selectedFile = files.IndexOf(ob1);
			selectedFileName = ob1.fullName;
			
			if(ob1.tags == null){
				ob1.tags = new List<string>();
				ob1.tags.Add("There are no tags yet :)");
			}
			selectedTags = ob1.tags;
			//Guess I'll have to create a command for this.
			//Done
		}
		
		//Now we need to add the ability to add tags
		//we already have a tag list, and we only need to apply it to a certain selected FileContainer 
		//in our list.
		//Now we know which file we edited, and we need a function to add a tag in there.
		/// <summary>
		/// Adds a tag to a fileContainer
		/// </summary>
		/// <param name="tag">A char array, that represents a string, that represents a tagh.</param>
		void addTagToFilecontainer(object tag){
			if(files[selectedFile].tags[0].Equals("There are no tags yet :)")){
				files[selectedFile].tags.RemoveAt(0);
			}
			selectedTags = files[selectedFile].addTag(tag as string);
		}
	}
}
