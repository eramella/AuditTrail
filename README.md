# Audit Trail
####Entities changes audit

This is the result of a job interview exercise I was asked to perform.

The purpose is to create an automatic system that logs any changes to any entity in your database.

Using [.NET Core](https://www.microsoft.com/net/core#windows) and [EntityFramework Core 1.0.0](https://docs.efproject.net/en/latest/) I created a little console app to seed the DB, randomly change the records and list the log.

Inspired by a [Julie Lerman](http://thedatafarm.com/) Pluralsight [course](https://app.pluralsight.com/library/search?q=Julie+Lerman) (can't remember which one) and a nice post by [Matthew P Jones](https://www.exceptionnotfound.net/entity-change-tracking-using-dbcontext-in-entity-framework-6/), it seems that overriding the SaveChanges method of the DbContex is probably the best way to go. 
In Julie course she has all her entities inherit from an interface that expose Id, DateModified, DateCreated and User and she automatically log this information within the entityobject.
By overriding the SaveChanges method we have access to all the properties that are being affected along with the OriginalValue and NewValues so we can log each property change accordingly (see AuditContext class).
