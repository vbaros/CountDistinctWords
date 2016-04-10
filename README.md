# CountDistinctWords
Counts distinct words in a text file using different functions for counting (including parallel) and different split methods.

Here is the output from all the included count and split functions. The best and worst times for each split function are highlighted.
The results are grouped by split functions and are executed in this order:

* SplitFunctions.SplitToWordsReplace
* SplitFunctions.SplitWithLinq
* SplitFunctions.SplitToWordsNormal
* SplitFunctions.SplitToWordsNormalDistinct
* SplitToWordsRegex

There are 2 examples:

1. Shows counting on a 100MB file consisting of words randomly generated from dictionary file.
2. Shows counting on a 100MB file consisting of words completely randomly generated.

![Dictionary generated](https://cloud.githubusercontent.com/assets/572428/14413142/d69da434-ff71-11e5-8eff-734ace2672f1.png)

![Randomly generated](https://cloud.githubusercontent.com/assets/572428/14413141/d69c55ac-ff71-11e5-9d22-bc81716f003f.png)
