## Design goals

This sample application aims at wasting no CPU cycles on waiting
for IO operations to complete.

Whenever a piece of code encounters a blocking operation, it will
relinquish the thread it is currently executing on. Then, once the
blocking operation has completed, the code will resume its
execution on another thread.

## Please show me

The following <a href="https://docs.google.com/presentation/embed?id=1DdCpFs9EYgmsFgImsViGXwinHkAsmOX702d7DtDIkEU&start=true&loop=false&delayms=3000">slideshow</a>
illustrates how threads are used to serve a simplified POST request. 

## Why bother?

Let's conduct a simple benchmark on 1,000 sets of database operations
scheduled for parallel execution. These operations will be the
insertion, selection, update and deletion of a database row.

### Strategy A - Hang on to the current thread

Number of operations: 1,000  
Total duration: 4917 milliseconds  
CPU utilization: 8 %  

### Strategy B - Resume on another thread

Number of operations: 1,000  
Total duration: 2778 milliseconds  
CPU utilization: 19 %

We can see that the benefits of not making the CPUs wait are twofold.

First, they can issue more concurrent IO requests, which reduces
the time required to perform the complete set of database operations.
Second, they are able to execute other lines of code, which augments
the CPU utilization and makes better use of the available hardware.