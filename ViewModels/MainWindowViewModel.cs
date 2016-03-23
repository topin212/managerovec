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
        public RelayCommand changePath { get; set; }
		#endregion
		
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
		
		
		List<DirInf> _allModifiedFiles;
		
		public List<DirInf> allModifiedFiles {
			get { return _allModifiedFiles; }
			set { _allModifiedFiles = value; }
		}
		
		int selectedFile; 
		ObservableCollection<string> _selectedTags;
		
		public ObservableCollection<string> selectedTags {
			get { return _selectedTags; }
			set { _selectedTags = value; 
				OnPropertyChanged("selectedTags");}
		}
		CommandLineClone ccv2 = new CommandLineClone(@"C:\");
		
		PathInfo path = new PathInfo(@"C:\");
		                             
		                             
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
		
		void FcliProcessorCommand(object obj){
			cliCommandHistory.Add(new CliCommandContainer(obj as string, ccv2.tryCommand(obj as string)));
			getFilesAndDirectories();
			OnPropertyChanged("cliCommandHistory");
			OnPropertyChanged("files");
		}
		
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
		
		void saveMyWorkPlease(object what=null){
			//well, we have a list of all files. We can save it all.
			//I do not care about that. For now.
			Serializator.Save(allModifiedFiles);
		}
		void loadMyWorkPlease(object what=null){
            try {
                allModifiedFiles = Serializator.Load();
            } catch (FileNotFoundException exc) {
                MessageBox.Show(exc.Message);
            } var kek = allModifiedFiles.Find(x=>x.path==currentPath);
			if(kek != null){
				files = new ObservableCollection<FileContainer>(kek.filesAndDirectories);
			}
		}
		
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
        
        void changeDir(object fileString) {
            foreach (var dirInf in allModifiedFiles) {
                if (dirInf.path.Equals(fileString as string)) {
                    currentPath = dirInf.path;
                    files = new ObservableCollection<FileContainer>(dirInf.filesAndDirectories);
                    OnPropertyChanged("currentPath");
                    OnPropertyChanged("files");
                    return;
                }
            }

            if ((fileString as string).Equals("..")) {
                try {
                    currentPath = previousPath;
                    getFilesAndDirectories();
                    OnPropertyChanged("currentPath");
                } catch (Exception E) {
                    MessageBox.Show(E.Message);
                }
            } else {
                try {
                    previousPath = currentPath;
                    currentPath = fileString as string;
                    getFilesAndDirectories();
                    OnPropertyChanged("currentPath");
                    OnPropertyChanged("files");
                } catch (Exception E) {
                    currentPath = previousPath;
                    OnPropertyChanged("currentPath");
                    MessageBox.Show(E.Message);
                }
            }
        }
	}
}
