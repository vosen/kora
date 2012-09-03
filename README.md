# Kora - library of integer dictionaries
This project contains implementations of various data structures in C# plus some benchmarking applications and tests.

## Implementation
All structures in this library implement ordered dictionaries, that means `System.Collections.Generic.IDictionary<int,T>` and few additional functions: `Higher(int x)`, `Lower(int x)`, `First()`, `Last()`. Library contains following data structures:

* van Emde Boas Tree
* x-fast trie (with standard hashing or dynamic perfect hashing)
* y-fast trie (with standard hashing or dynamic perfect hashing)

Some data structures required red-black tree. Implementation comes from [Mono](http://www.mono-project.com). Also it is used as a point of reference in benchmarking.

## Performance
Performance results of all important functions. All benchmarks were run on structures of size from 100 000 elements to 1 000 000 elements containing randomly picked elements. In case of find 100 000, search operation was performed 100 000 times. Time is in milliseconds, memory usage is in megabytes. 
### Add
![Graph for add](http://img189.imageshack.us/img189/5109/largeadd.png)

### Remove
![Graph for remove](http://img4.imageshack.us/img4/8749/largedel.png)

### Find
![Graph for find](http://img27.imageshack.us/img27/5484/largefind.png)

### Higher
![Graph for higher](http://img842.imageshack.us/img842/2202/largesucc.png)

### Memory usage
![Memory usage graph](http://img208.imageshack.us/img208/5514/largemem.png)
