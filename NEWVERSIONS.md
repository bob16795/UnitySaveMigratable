1. copy SaveTemplate.cs.tmp to saveVx.cs
2. edit data in SaveVx.cs to reflect save version
3. add a constructor in latest from previous save version.
4. write load, read,
5. add data added to save format. __in public vars!__
6. edit SaveReadWrite.cs
replace:
```cs
private SaveVx-1 save;
```
with:
```cs
private SaveVx save;
```

and add save read to switch in read

7. edit MigrationManager.cs
add to the else thing
```cs
else if (save.version == x-1) result = (ISaveFileVersion)new SaveVx((SaveVx-1)save);
```