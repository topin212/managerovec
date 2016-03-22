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
using System.Collections.ObjectModel;
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
				OnPropertyChanged("selectedFileName");}
		}
		
		string _currentPath;
		public string currentPath {
			get { return _currentPath; }
			set { _currentPath = value; 
				OnPropertyChanged("currentPath");}
		}
		
		string _previousPath;
		public string previousPath {
			get { return _previousPath; }
			set { _previousPath = value; 
				OnPropertyChanged("previousPath");}
		}
		
		#region Some commands here:
		public RelayCommand fileListViewDoubleClickCommand { get; set; }
		public RelayCommand cliProcessorCommand { get; set; }
		public RelayCommand fileListViewSelectionChangedCommand { get; set; }
		public RelayCommand addTagCommand { get; set; }
		public RelayCommand removeTagCommand { get; set; }
		public RelayCommand searchTagCommand { get; set; }
		public RelayCommand saveTagsCommand { get; set; }
		public RelayCommand loadTagsCommand { get; set; }
		#endregion
		
		//I will need a public List<> property to display files.
		//should I use list<list<file>>? Nope, I think this is a bad idea, and I will figure out a better way
		//Actually, this idea is not that bad. Becausem when u'r
		ObservableCollection<FileContainer> _files;
		public ObservableCollection<FileContainer> files {
			get { return _files; }
			set { _files = value; 
				OnPropertyChanged("files");}
		}
		
		ObservableCollection<CliCommandContainer> _cliCommandHistory;
		public ObservableCollection<CliCommandContainer> cliCommandHistory {
			get { return _cliCommandHistory; }
			set { _cliCommandHistory = value; 
				OnPropertyChanged("cliCommandHistory");}
		}
		
		//And the most doubt part - should I do this? this will help serializing. Ok, nevermind.
		
		List<DirInf> _allModifiedFiles;
		
		public List<DirInf> allModifiedFiles {
			get { return _allModifiedFiles; }
			set { _allModifiedFiles = value; }
		}
		
		//id of that selected file, which is needed later on, while saving the tags.
		int selectedFile; 
		ObservableCollection<string> _selectedTags;
		
		public ObservableCollection<string> selectedTags {
			get { return _selectedTags; }
			set { _selectedTags = value; 
				OnPropertyChanged("selectedTags");}
		}
		//so, we have our properties, now we need to fill them up, and then bind them to 
		//both listViews, and we can test that.
		
		//I will need an instance of my old ConsoleClone to help me with dat task!
		CommandLineClone ccv2 = new CommandLineClone(@"C:\");
		
		//and for some extra path manipulation I need this:
		PathInfo path = new PathInfo(@"C:\");
		//just plugged in the default path
		                             
		                             
		public MainWindowViewModel()
		{
			allModifiedFiles = new List<DirInf>();
			#region populating listView with data
			//initialize the Path variable :)
			currentPath = @"C:\";
			//here are the calls for populating lists with data.
			files = new ObservableCollection<FileContainer>();
			cliCommandHistory = new ObservableCollection<CliCommandContainer>();
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
			#region adding and removing tags
			addTagCommand = new RelayCommand(addTagToFilecontainer);
			removeTagCommand = new RelayCommand(removeTagFromFileContainer);
			#endregion
			#region searchin Tags
			searchTagCommand = new RelayCommand(search);
			saveTagsCommand = new RelayCommand(saveMyWorkPlease);
			loadTagsCommand = new RelayCommand(loadMyWorkPlease);
			#endregion
			
			#region fileListView Extra
			fileListViewDoubleClickCommand = new RelayCommand(listViewDoubleClick);
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
		/// </summary>
		void getFilesAndDirectories(){
			ObservableCollection<FileContainer> lst1 = new ObservableCollection<FileContainer>();
			lst1.Add(new FileContainer("..", "up"));
			
			var currentDir = new DirectoryInfo(currentPath);
			DirectoryInfo[] internalDirs = currentDir.GetDirectories();
			FileInfo[] internalFiles = currentDir.GetFiles();
			
			foreach (var directory in internalDirs) {
				lst1.Add(new FileContainer(directory.Name, directory.FullName, "<dir>", directory.Attributes.ToString(), directory.CreationTime));
			}
			//now, we populated that with directories, but the files remain untouched...
			foreach (var file in internalFiles) {
				lst1.Add(new FileContainer(file.Name, file.FullName, "<" + file.Extension + ">", file.Attributes.ToString(), file.CreationTime));
			}
			
			_files.Clear();
			_files = new ObservableCollection<FileContainer>(lst1);
			OnPropertyChanged("files");
			//populated? great. Now we need to take this, and bind it to the fileListView.
		}
		
		void getTagsFromFile(){
			var loadedFiles = new List<DirInf>();
			try {
				loadedFiles = Serializator.Load();
			} catch (Exception e) {	
				Console.WriteLine(e.Message);
			}
			
			//if (File.Exists(Directory.GetCurrentDirectory() + @"\tags.tags")){
			//	lst1 = Serializator.Load();
			//}
			//var newList = new List<FileContainer>();
			//newList.Add(new FileContainer("..", "up"));
			//newList.AddRange(lst1.Union(lst2).ToList());
			//_files.Clear();
		}
		
		//So, the last thing = is to fix changing directories and load tags into _files correctly.
		
		//well, I'll handle double-clicks a bit later. 
		
		//Okay, Nevermind. This doesn't work yet.
		//So, let's pass to the selection event.
		void FcliProcessorCommand(object obj){
			cliCommandHistory.Add(new CliCommandContainer(obj as string, ccv2.tryCommand(obj as string)));
			getFilesAndDirectories();
			OnPropertyChanged("cliCommandHistory");
			OnPropertyChanged("files");
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
		void changeInternalSelection(object selectedItem){
			//Now I need to find an item with that name in my list. Which could be done by Linq
			var ob1 = selectedItem as FileContainer;
			selectedFile = files.IndexOf(ob1);
			selectedFileName = ob1.fullName;
			
			if(ob1.tags == null){
				ob1.tags = new ObservableCollection<string>();
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
			try{
				if(files[selectedFile].tags[0].Equals("There are no tags yet :)")){
					files[selectedFile].tags.RemoveAt(0);
				}
				selectedTags = files[selectedFile].addTag(tag as string);
				if(allModifiedFiles.Find(x=>x.path==currentPath) == null)
					allModifiedFiles.Add(new DirInf(currentPath, files.ToList()));
				OnPropertyChanged("selectedTags");
			}catch(NullReferenceException exc){
				Console.WriteLine(exc.Message + "\n\t While writing tag");
			}
			
		}
		
		//OK, nevermind all that changes. Let's just hop over to removing tags and saving them.
		/// <summary>
		/// Removes a tag from a fileContainer
		/// </summary>
		/// <param name="tag">A string tag to be removed from the collection.</param>
		void removeTagFromFileContainer(object tag){
			//if this is the last tag, insert dummy tag
			if(files[selectedFile].tags.Count == 1){
				files[selectedFile].tags.Add("There are no tags yet :)");
			}
			selectedTags = files[selectedFile].removeTag(tag as string);
			OnPropertyChanged("selectedTags");
		}
		
		//with this last thing done, we need to save our work somehow.
		//and then load it. Somehow.
		void saveMyWorkPlease(object what=null){
			//well, we have a list of all files. We can save it all.
			//I do not care about that. For now.
			Serializator.Save(allModifiedFiles);
		}
		void loadMyWorkPlease(object what=null){
			allModifiedFiles = Serializator.Load();
			var kek = allModifiedFiles.Find(x=>x.path==currentPath);
			if(kek != null){
				files = new ObservableCollection<FileContainer>(kek.filesAndDirectories);
			}
		}
		//well, it saves progress, properly loads tags, that's cool. now I need searching!
		
		void search(object Tag){
			var found = new ObservableCollection<FileContainer>();
			foreach (var file in files) {
				if(file.tags != null){
				foreach (var tag in file.tags) {
					if(tag.Equals(Tag as string)){
						found.Add(file);
						break;
					}
					}
				}
			}
			files = found;
			OnPropertyChanged("files");
		}
		//as search with tags is done, I have no more work to do except for the usability.
		//checking out other directories and drives would be cool.
		//so, let's get it started right through the rpain :D
		
		//It's time to handle DoubleClicks!
		void listViewDoubleClick(object obj)
		{
			foreach (var dirInf in allModifiedFiles) {
				if(dirInf.path.Equals((obj as FileContainer).filename)){
					currentPath = dirInf.path;
					files = new ObservableCollection<FileContainer>(dirInf.filesAndDirectories);
					OnPropertyChanged("currentPath");
					OnPropertyChanged("files");
					return;
				}
			}
			
			if((obj as FileContainer).filename.Equals("..")){
				try {
					currentPath = previousPath;
					getFilesAndDirectories();
					OnPropertyChanged("currentPath");
				} catch (Exception E) {
					MessageBox.Show(E.Message);
				}
			}else{
				try{
					previousPath = currentPath;
					currentPath = (obj as FileContainer).fullName;
					getFilesAndDirectories();
					OnPropertyChanged("currentPath");
					OnPropertyChanged("files");
				}
				catch(Exception E){
					currentPath = previousPath;					
					OnPropertyChanged("currentPath");
					MessageBox.Show(E.Message);
				}
			}
			//DebugInfo
			path.path=currentPath;
			path.getNumberOfLevels();
		}
	}
}
