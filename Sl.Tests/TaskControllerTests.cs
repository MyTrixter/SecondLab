using Sl.Domain;

namespace Sl.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("task category")]
        public void AddTaskCategory_ShouldAddCategory(string categoryName)
        {
            // arrange
            var taskController = new TaskController();

            // act
            taskController.AddTaskCategory(categoryName);

            // assert
            Assert.IsTrue(taskController.Categories.Contains(categoryName));
        }

        [Test]
        public void AddTaskCategory_TryAddEmptyCategory_ShouldThrowInvalidOperationException()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = string.Empty;

            // act

            // assert
            Assert.Throws<InvalidOperationException>(() => taskController.AddTaskCategory(categoryName));
        }

        [Test]
        public void AddTaskCategory_TryAddTwoCategoryWithSameName_ShouldThrowInvalidOperationException()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = "categoryName";

            // act
            taskController.AddTaskCategory(categoryName);

            // assert
            Assert.Throws<InvalidOperationException>(() => taskController.AddTaskCategory(categoryName));
        }

        [Test]
        public void RemoveCategory_ShouldRemoveCategory()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = "categoryName";

            // act
            taskController.AddTaskCategory(categoryName);
            taskController.RemoveTaskCategory(categoryName);

            // assert
            Assert.IsFalse(taskController.Categories.Contains(categoryName));
        }

        [TestCase("category 1")]
        [TestCase("category 2")]
        public void RemoveCategory_TryRemoveNotExistCategory_ShouldThrowInvalidOperationException(string categoryName)
        {
            // arrange
            var taskController = new TaskController();

            // act

            // assert
            Assert.Throws<InvalidOperationException>(() => taskController.RemoveTaskCategory(categoryName));
        }

        [Test]
        public void AddTask_ShouldAddTask()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = "category";
            var task = new Domain.Task("title", "description", categoryName);

            // act
            taskController.AddTaskCategory(categoryName);
            taskController.AddTask(task);

            // assert
            Assert.IsTrue(taskController.Tasks.Contains(task));
        }

        [Test]
        public void AddTask_TryAddTaskWithNotExistCategory_ShouldThrowInvalidOperationException()
        {
            // arrange
            var taskController = new TaskController();
            var task = new Domain.Task("title", "description", "category");

            // act

            // assert
            Assert.Throws<InvalidOperationException>(() => taskController.AddTask(task));
        }

        [Test]
        public void AddTask_TryAddEmptyTask_ShouldThrowInvalidOperationException()
        {
            // arrange
            var taskController = new TaskController();
            var task = new Domain.Task("", "", "");

            // act

            // assert
            Assert.Throws<InvalidOperationException>(() => taskController.AddTask(task));
        }

        [Test]
        public void RemoveTask_TryRemoveNotExistTask_ShouldThrowInvalidOperationException()
        {
            // arrange
            var taskController = new TaskController();

            // act

            // assert
            Assert.Throws<InvalidOperationException>(() => taskController.RemoveTask("title"));
        }

        [Test]
        public void RemoveTask_ShouldRemoveTask()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = "category";
            var task = new Domain.Task("title", "description", categoryName);

            // act
            taskController.AddTaskCategory(categoryName);
            taskController.AddTask(task);
            taskController.RemoveTask(task.Title);

            // assert
            Assert.IsFalse(taskController.Tasks.Contains(task));
        }

        [Test]
        public void AddTaskToFavorite_ShouldAddTaskToFavorite()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = "category";
            var task = new Domain.Task("title", "description", categoryName);

            // act
            taskController.AddTaskCategory(categoryName);
            taskController.AddTask(task);
            taskController.AddTaskToFavorie(task.Title);

            // assert
            Assert.IsTrue(taskController.Tasks.First(x => x.Title == task.Title).IsFavorite);
        }

        [Test]
        public void RemoveTaskOfFavorite_ShouldRemoveTaskOfFavorite()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = "category";
            var task = new Domain.Task("title", "description", categoryName, true);

            // act
            taskController.AddTaskCategory(categoryName);
            taskController.AddTask(task);
            taskController.RemoveTaskOfFavorie(task.Title);

            // assert
            Assert.IsFalse(taskController.Tasks.First(x => x.Title == task.Title).IsFavorite);
        }

        [Test]
        public void GetAllTasksByCategoryOrderedDescending_ShoudReturnAllTasksByCategoryOrderedDescending()
        {
            // arrange
            var taskController = new TaskController();
            var firstCategory = "first category";
            var secondCategory = "second category";
            int numberOfFirstCategoryTasks = 3;
            int numberOfSecondCategoryTasks = 2;

            // act
            taskController.AddTaskCategory(firstCategory);
            taskController.AddTaskCategory(secondCategory);

            for (int i = 0; i < numberOfFirstCategoryTasks; i++)
            {
                taskController.AddTask(new Domain.Task
                    (Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), firstCategory));
            }

            for (int i = 0; i < numberOfSecondCategoryTasks; i++)
            {
                taskController.AddTask(new Domain.Task
                    (Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), secondCategory));
            }

            var tasksWithFirstCategory = taskController.GetAllTasksByCategoryOrderedDescending(firstCategory);
            var tasksWithSecondCategory = taskController.GetAllTasksByCategoryOrderedDescending(secondCategory);

            // assert
            Assert.IsTrue(tasksWithFirstCategory.Count == numberOfFirstCategoryTasks);
            Assert.IsTrue(tasksWithSecondCategory.Count == numberOfSecondCategoryTasks);
        }

        [Test]
        public void GetAllFavoriteTasksOrderedDescending_ShoudReturnAllFavoriteTasksOrderedDescending()
        {
            // arrange
            var taskController = new TaskController();
            var categoryName = "first category";
            int numberOfFavoriteTasks = 3;
            int numberOfNotFavoriteTasks = 2;

            // act
            taskController.AddTaskCategory(categoryName);

            for (int i = 0; i < numberOfFavoriteTasks; i++)
            {
                taskController.AddTask(new Domain.Task
                    (Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), categoryName, true));
            }

            for (int i = 0; i < numberOfNotFavoriteTasks; i++)
            {
                taskController.AddTask(new Domain.Task
                    (Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), categoryName));
            }

            var favoriteTasks = taskController.GetAllFavoriteTasksOrderedDescending();

            // assert
            Assert.IsTrue(favoriteTasks.Count == numberOfFavoriteTasks);
            Assert.IsTrue(taskController.Tasks.Count == numberOfNotFavoriteTasks + numberOfFavoriteTasks);
        }
    }
}