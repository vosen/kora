import os, re, csv
from matplotlib import pyplot

markers=['x', 'd', 's', 'o', '3', '^', 'v']

def load_files(file_list):
    perf =[]
    for file in file_list:
        x = []
        y = []
        reader = csv.reader(open(file))
        for row in reader:
            x.append(int(row[0])/1000)
            y.append(int(row[1]))
        perf.append((file, x, y))
    return perf
    

files=filter(lambda x: not re.match(".*\.py", x), os.listdir("."))
add_files = filter(lambda x: re.match(".*-ADD", x), files)
del_files = filter(lambda x: re.match(".*-DEL", x), files)
add_perf = sorted(load_files(add_files), key = lambda perf: perf[2][-1], reverse=True)
del_perf = sorted(load_files(del_files), key = lambda perf: perf[2][-1], reverse=True)

pyplot.figure(figsize=(12,8))
i = 0
for val in add_perf:
    pyplot.plot(val[1], val[2], label=val[0][:-4], linestyle='--', marker=markers[i])
    i+=1
pyplot.legend(loc=2)
pyplot.savefig("add.svg", bbox_inches='tight')

pyplot.clf()
pyplot.cla()

pyplot.figure(figsize=(12,8))
i = 0
for val in del_perf:
    pyplot.plot(val[1], val[2], label=val[0][:-4], linestyle='--', marker=markers[i])
    i+=1
pyplot.legend(loc=2)
pyplot.savefig("del.svg", bbox_inches='tight')