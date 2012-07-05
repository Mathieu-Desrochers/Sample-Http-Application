## Design goals

This application will never hang to a CPU while waiting for an IO operation 
to complete. When a piece of code encounters a blocking operation, it will 
release its thread. Once the blocking operation has completed, the piece 
of code will resume on a different thread.

## Please show me

The following <a href="https://docs.google.com/presentation/embed?id=1DdCpFs9EYgmsFgImsViGXwinHkAsmOX702d7DtDIkEU&start=true&loop=false&delayms=3000" target="_blank">slideshow</a>
illustrates how threads are used to serve a simplified POST request. 

## Why bother

A simple benchmark operation was used to insert, select, update
and then delete a database row. The following results were obtained
when running the operation 1,000 times in parallel.

### Strategy A - Hang on to the current thread

Total duration: 4917 milliseconds  
CPU utilization: 8 %  

### Strategy B - Resume on another thread

Total duration: 2778 milliseconds  
CPU utilization: 19 %

We can see that the benefits of keeping the CPUs available are twofold.
Firstly, the CPUs can issue more concurrent IO requests, which reduces the
total duration required for their completion. Secondly, the CPUs can execute
other lines of code, which augments their utilization and makes better
use of the available hardware.