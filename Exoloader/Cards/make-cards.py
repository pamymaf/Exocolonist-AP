from pprint import pprint
import json
import csv
import os

template = {}
with open("template.json") as f:
  template = json.loads(f.read())

jobdata = {}
with open("Exocolonist - jobs.tsv") as f:
  dictReader = csv.DictReader(f, delimiter='\t')
  for row in dictReader:
    jobdata[row["ID"]] = row


for job in jobdata:
  job = jobdata[job]
  thisTemplate = template.copy()
  if job["ID"] == "photophonor":
    job["ID"] = "playPhotophonor"
  thisTemplate["ID"] = job["ID"]
  thisTemplate["Name"] = job["Name"]
  with open(f"{job['ID']}.json", 'w+') as f:
    f.write(json.dumps(thisTemplate))
  cmd = f"cp template.png {job['ID']}.png"
  os.system(cmd)