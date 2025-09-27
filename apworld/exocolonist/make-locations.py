from pprint import pprint

characters = ["Anemone", "Cal", "Marz", "Tammy", "Tang", "Dys", "Sym", "Nomi", "Rex", "Vace"]
i = 500

end = {}
for chara in characters:
  for j in range(10,110,10):
    end[f"{chara} {j}"] = i
    i = i+1
  end[f"{chara} Date"] = i
  i = i+1

pprint(end)