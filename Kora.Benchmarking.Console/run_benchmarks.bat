@ECHO OFF

:: small-add
ECHO Measuring add for 10.000 - 100.000 (slow)
mkdir small-add
Kora.Benchmarking.Console.exe small-add 10000 10 10000 S a
python make_graph.py small-add

:: small-del
ECHO Measuring delete for 10.000 - 100.000 (slow)
mkdir small-del
Kora.Benchmarking.Console.exe small-del 10000 10 10000 S d
python make_graph.py small-del

:: large-add
ECHO Measuring add for 100.000 - 1.000.000 (fast)
mkdir large-add
Kora.Benchmarking.Console.exe large-add 100000 10 100000 F a
python make_graph.py large-add

:: large-del
ECHO Measuring delete for 100.000 - 1.000.000 (fast)
mkdir large-del
Kora.Benchmarking.Console.exe large-del 100000 10 100000 F d
python make_graph.py large-del

:: large-find
ECHO Measuring search for 100.000 - 1.000.000 (fast)
mkdir large-find
Kora.Benchmarking.Console.exe large-find 100000 10 100000 F f 100000
python make_graph.py large-find

:: large-succ
ECHO Measuring successor for 100.000 - 1.000.000 (fast)
mkdir large-succ
Kora.Benchmarking.Console.exe large-succ 100000 10 100000 F s 100000
python make_graph.py large-succ

:: large-mem
ECHO Measuring memory for 100.000 - 1.000.000 (fast)
mkdir large-mem
Kora.Benchmarking.Console.exe large-mem 100000 10 100000 F m
python make_graph.py large-mem