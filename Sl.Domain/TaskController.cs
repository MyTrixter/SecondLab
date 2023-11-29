namespace Sl.Domain
{
    public class TaskController
    {
        public List<Task> Tasks { get; }
        public List<string> Categories { get; }

        public TaskController()
        {
            Tasks = new();
            Categories = new();
        }

        private bool TaskIsExist(string taskTitle)
        {
            return Tasks.Where(x => x.Title == taskTitle).Any();
        }

        private bool TaskCategoryIsExist(string categoryName)
        {
            return Categories.Contains(categoryName);
        }

        public void AddTaskCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new InvalidOperationException($"A task category can not be empty.");
            }

            if (TaskCategoryIsExist(categoryName))
            {
                throw new InvalidOperationException($"A task category with name {categoryName} already exist.");
            }

            Categories.Add(categoryName);
        }

        public void RemoveTaskCategory(string categoryName)
        {
            if (TaskCategoryIsExist(categoryName) == false)
            {
                throw new InvalidOperationException($"A task category with name {categoryName} doesnt exist.");
            }

            Categories.Remove(categoryName);

        }

        public void AddTask(Task task)
        {
            if (task.IsValid() == false)
            {
                throw new InvalidOperationException($"A task is not valid.");
            }
            
            if (TaskIsExist(task.Title))
            {
                throw new InvalidOperationException($"A task with title {task.Title} already exist.");
            }

            if (TaskCategoryIsExist(task.Category) == false)
            {
                throw new InvalidOperationException($"A task category with name {task.Category} doesnt exist.");
            }

            Tasks.Add(task);
        }

        public void RemoveTask(string taskTitle)
        {
            if (TaskIsExist(taskTitle) == false)
            {
                throw new InvalidOperationException($"A task with title {taskTitle} not exist");
            }

            Tasks.Remove(Tasks.First(x => x.Title == taskTitle));
        }

        public void AddTaskToFavorie(string taskTitle)
        {
            if (TaskIsExist(taskTitle) == false)
            {
                throw new InvalidOperationException($"A task with title {taskTitle} not exist");
            }

            Tasks.First(x => x.Title == taskTitle).IsFavorite = true;
        }

        public void RemoveTaskOfFavorie(string taskTitle)
        {
            if (TaskIsExist(taskTitle) == false)
            {
                throw new InvalidOperationException($"A task with title {taskTitle} not exist");
            }

            Tasks.First(x => x.Title == taskTitle).IsFavorite = false;
        }

        public List<Task> GetAllFavoriteTasksOrderedDescending()
        {
            return Tasks.Where(x => x.IsFavorite == true)
                .OrderBy(x => x.Title)
                .ToList();
        }

        public List<Task> GetAllTasksByCategoryOrderedDescending(string categoryName)
        {
            if (TaskCategoryIsExist(categoryName) == false)
            {
                throw new InvalidOperationException($"A task category with name {categoryName} doesnt exist.");
            }

            return Tasks.Where(x => x.Category == categoryName)
                .OrderBy(x => x.Title)
                .ToList();
        }
    }
}
