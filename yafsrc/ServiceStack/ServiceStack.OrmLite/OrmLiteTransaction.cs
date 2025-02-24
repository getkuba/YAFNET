﻿// ***********************************************************************
// <copyright file="OrmLiteTransaction.cs" company="ServiceStack, Inc.">
//     Copyright (c) ServiceStack, Inc. All Rights Reserved.
// </copyright>
// <summary>Fork for YetAnotherForum.NET, Licensed under the Apache License, Version 2.0</summary>
// ***********************************************************************
using System;
using System.Data;
using ServiceStack.Data;

namespace ServiceStack.OrmLite;

/// <summary>
/// Class OrmLiteTransaction.
/// Implements the <see cref="System.Data.IDbTransaction" />
/// Implements the <see cref="ServiceStack.Data.IHasDbTransaction" />
/// </summary>
/// <seealso cref="System.Data.IDbTransaction" />
/// <seealso cref="ServiceStack.Data.IHasDbTransaction" />
public class OrmLiteTransaction : IDbTransaction, IHasDbTransaction
{
    /// <summary>
    /// Gets or sets the transaction.
    /// </summary>
    /// <value>The transaction.</value>
    public IDbTransaction Transaction { get; set; }

    /// <summary>
    /// Gets the database transaction.
    /// </summary>
    /// <value>The database transaction.</value>
    public IDbTransaction DbTransaction => Transaction;

    /// <summary>
    /// The database connection
    /// </summary>
    private readonly IDbConnection db;

    /// <summary>
    /// Gets the database connection.
    /// </summary>
    /// <value>The database connection.</value>
    public IDbConnection Db => db;

    /// <summary>
    /// Creates the specified database.
    /// </summary>
    /// <param name="db">The database.</param>
    /// <param name="isolationLevel">The isolation level.</param>
    /// <returns>OrmLiteTransaction.</returns>
    public static OrmLiteTransaction Create(IDbConnection db, IsolationLevel? isolationLevel = null)
    {
        var dbTrans = isolationLevel != null
                          ? db.BeginTransaction(isolationLevel.Value)
                          : db.BeginTransaction();

        Diagnostics.OrmLite.WriteTransactionOpen(dbTrans.IsolationLevel, db);
        var trans = new OrmLiteTransaction(db, dbTrans);

        return trans;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrmLiteTransaction" /> class.
    /// </summary>
    /// <param name="db">The database.</param>
    /// <param name="transaction">The transaction.</param>
    public OrmLiteTransaction(IDbConnection db, IDbTransaction transaction)
    {
        this.db = db;
        this.Transaction = transaction;

        //If OrmLite managed connection assign to connection, otherwise use OrmLiteContext
        if (this.db is ISetDbTransaction ormLiteConn)
        {
            ormLiteConn.Transaction = this.Transaction = transaction;
        }
        else
        {
            OrmLiteContext.TSTransaction = this.Transaction = transaction;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        try
        {
            Transaction.Dispose();
        }
        finally
        {
            if (this.db is ISetDbTransaction ormLiteConn)
            {
                ormLiteConn.Transaction = null;
            }
            else
            {
                OrmLiteContext.TSTransaction = null;
            }
        }
    }

    /// <summary>
    /// Commits the database transaction.
    /// </summary>
    public void Commit()
    {
        var isolationLevel = Transaction.IsolationLevel;
        var id = Diagnostics.OrmLite.WriteTransactionCommitBefore(isolationLevel, db);
        Exception e = null;

        try
        {
            Transaction.Commit();
        }
        catch (Exception ex)
        {
            e = ex;
            throw;
        }
        finally
        {
            if (e != null)
            {
                Diagnostics.OrmLite.WriteTransactionCommitError(id, isolationLevel, db, e);
            }
            else
            {
                Diagnostics.OrmLite.WriteTransactionCommitAfter(id, isolationLevel, db);
            }
        }
    }

    /// <summary>
    /// Rolls back a transaction from a pending state.
    /// </summary>
    public void Rollback()
    {
        var isolationLevel = Transaction.IsolationLevel;
        var id = Diagnostics.OrmLite.WriteTransactionRollbackBefore(isolationLevel, db, null);
        Exception e = null;

        try
        {
            Transaction.Rollback();
        }
        catch (Exception ex)
        {
            e = ex;
            throw;
        }
        finally
        {
            if (e != null)
            {
                Diagnostics.OrmLite.WriteTransactionRollbackError(id, isolationLevel, db, null, e);
            }
            else
            {
                Diagnostics.OrmLite.WriteTransactionRollbackAfter(id, isolationLevel, db, null);
            }
        }
    }

    /// <summary>
    /// Specifies the Connection object to associate with the transaction.
    /// </summary>
    /// <value>The connection.</value>
    public IDbConnection Connection => Transaction.Connection;

    /// <summary>
    /// Gets the isolation level.
    /// </summary>
    /// <value>The isolation level.</value>
    public IsolationLevel IsolationLevel => Transaction.IsolationLevel;
}