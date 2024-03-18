using System.Data.Common;
using System.Text;
using LCPC.Share;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LCPC.Infrastructure.Cores;

public class IntertorCommand:DbCommandInterceptor
{
    
    public override DbCommand CommandInitialized(CommandEndEventData eventData, DbCommand result)
    {

        string executeSql = result.CommandText;
        var command = result;
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("-------原始语句[NonQueryExecuting]-----------");
        sb.AppendLine(command.CommandText);
        sb.AppendLine("-------原始语句结束[NonQueryExecuting]-----------");
        sb.AppendLine($"------可执行的语句[NonQueryExecuting]-------");
         
        foreach (DbParameter item in command.Parameters)
        {
            var name = item.ParameterName;
            var value = Convert.ToString(item.Value);
            executeSql= executeSql.Replace(name, value);
        }

        sb.AppendLine(executeSql);
        sb.AppendLine("------语句拦截结束[NonQueryExecuting]-----");
        LoggerManager.LoggerInfo(sb.ToString());
        return base.CommandInitialized(eventData, result);
    }

    public override InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
    {
        return base.CommandCreating(eventData, result);
    }

    public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        string executeSql = command.CommandText;
     
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("-------原始语句[NonQueryExecuting]-----------");
        sb.AppendLine(command.CommandText);
        sb.AppendLine("-------原始语句结束[NonQueryExecuting]-----------");
        sb.AppendLine($"------可执行的语句[NonQueryExecuting]-------");
        foreach (DbParameter item in command.Parameters)
        {
            var name = item.ParameterName;
            var value = Convert.ToString(item.Value);
            executeSql= executeSql.Replace(name, value);
        }

        sb.AppendLine(executeSql);
        sb.AppendLine("------语句拦截结束[NonQueryExecuting]-----");
        LoggerManager.LoggerInfo(sb.ToString());
        return base.NonQueryExecuting(command, eventData, result);
    }

    // private readonl
    public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
    {
        var command = result;
        if (command != null)
        {
            string executeSql = command.CommandText;
     
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-------原始语句[CommandCreated]-----------");
            sb.AppendLine(command.CommandText);
            sb.AppendLine("-------原始语句结束[CommandCreated]-----------");
        
        
            sb.AppendLine($"------可执行的语句[CommandCreated]-------");
            foreach (DbParameter item in command.Parameters)
            {
                var name = item.ParameterName;
                var value = Convert.ToString(item.Value);
                executeSql= executeSql.Replace(name, value);
            }

            sb.AppendLine(executeSql);
            sb.AppendLine("------语句拦截结束[CommandCreated]-----");
            LoggerManager.LoggerInfo(sb.ToString());
        }
    
        return base.CommandCreated(eventData, result);
    }

    public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        string executeSql = command.CommandText;
     
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("-------原始语句[NonQueryExecuted]-----------");
        sb.AppendLine(command.CommandText);
        sb.AppendLine("-------原始语句结束[NonQueryExecuted]-----------");
        sb.AppendLine($"------可执行的语句[NonQueryExecuted]-------");
        foreach (DbParameter item in command.Parameters)
        {
            var name = item.ParameterName;
            var value = Convert.ToString(item.Value);
            executeSql= executeSql.Replace(name, value);
        }

        sb.AppendLine(executeSql);
        sb.AppendLine("------语句拦截结束[NonQueryExecuted]-----");
        LoggerManager.LoggerInfo(sb.ToString());
        return base.NonQueryExecuted(command, eventData, result);
    }


    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        Console.WriteLine($"CommandFailed[拦截的语句]: {command.CommandText}");
        base.CommandFailed(command, eventData);
    }
}