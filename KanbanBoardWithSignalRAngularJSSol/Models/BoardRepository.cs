using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanBoardWithSignalRAngularJSSol.Models
{
    public class BoardRepository
    {
        public List<Column> GetColumns()
        {
            if (HttpContext.Current.Cache["columns"] == null)
            {
                var columns = new List<Column>();
                var tasks = new List<Task>();
                var subTasks = new List<SubTasks>();

                // Adicionando Tarefas e Subtarefas
                for (int i = 1; i < 6; i++)
                {
                    tasks.Add(new Task { ColumnId = 1, Id = i, Name = "Task " + i, Description = "Task " + i + " Description" });
                    for (int j = 1; j < 6; j++)
                    {
                        subTasks.Add(new SubTasks { Id = i * 1000 + j, IdTask = i, Subtask = "SubTask " + i * 1000 + j + " Description", HoraInclusao = DateTime.Now });
                    }
                }
                tasks.Add(new Task { ColumnId = 2, Id = 6, Name = "Task " + 6, Description = "Task " + 6 + " Description" });


                // Adicionando colunas
                columns.Add(new Column { Description = "to do column", Id = 1, Name = "Backlog", Tasks = tasks });
                columns.Add(new Column { Description = "in progress column", Id = 2, Name = "A Iniciar", Tasks = new List<Task>() });
                columns.Add(new Column { Description = "test column", Id = 3, Name = "Em Progresso", Tasks = new List<Task>() });
                columns.Add(new Column { Description = "done column", Id = 4, Name = "Concluído", Tasks = new List<Task>() });


                HttpContext.Current.Cache["columns"] = columns;
            }
            return (List<Column>)HttpContext.Current.Cache["columns"];
        }

        public Column GetColumn(int colId)
        {
            return (from c in this.GetColumns()
                    where c.Id == colId
                    select c).FirstOrDefault();
        }

        public Task GetTask(int taskId)
        {
            var columns = this.GetColumns();
            foreach (var c in columns)
            {
                foreach (var task in c.Tasks)
                {
                    if (task.Id == taskId)
                        return task;
                }
            }

            return null;
        }

        public void MoveTask(int taskId, int targetColId)
        {
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);

            // Add task to the target column
            var task = this.GetTask(taskId);
            var sourceColId = task.ColumnId;
            task.ColumnId = targetColId;
            targetColumn.Tasks.Add(task);

            // Remove task from source column
            var sourceCol = this.GetColumn(sourceColId);
            sourceCol.Tasks.RemoveAll(t => t.Id == taskId);

            // Update column collection
            columns.RemoveAll(c => c.Id == sourceColId || c.Id == targetColId);
            columns.Add(targetColumn);
            columns.Add(sourceCol);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        private void UpdateColumns(List<Column> columns)
        {
            HttpContext.Current.Cache["columns"] = columns;
        }
    }
}