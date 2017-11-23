# HyperDB

## A search tree based storage structure or hash algorithm


```
                      [root]
                        |
       +-------------+----------+------+
       0             1          2      3
       |             |          |      |
  key0 = 0???   key0 = 0???    ...    ...
  key1 = 0???   key1 = 1???    ...    ...
       |             |
      ...            |
                     |
     +-------------+----------+------+
     0             1          2      3
     |             |          |      |
key0 = 00??   key0 = 00??    ...    ...
key1 = 10??   key1 = 11??    ...    ...
       
       
```
