import os, sys, re, csv
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
    
def listdir_fullpath(d):
    return [os.path.join(d, f) for f in os.listdir(d)]
    
path = sys.argv[1]

files=filter(lambda x: not re.match(".*\.", x), listdir_fullpath(path))
perf_values = sorted(load_files(files), key = lambda perf: perf[2][-1], reverse=True)

pyplot.figure(figsize=(12,8))
i = 0
for val in perf_values:
    pyplot.plot(val[1], val[2], label=val[0][len(path)+1:], linestyle='--', marker=markers[i])
    i+=1
pyplot.legend(loc=2)
pyplot.savefig(os.path.join(path, "graph.svg"), bbox_inches='tight')