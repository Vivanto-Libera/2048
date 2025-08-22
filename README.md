# 2048
利用Godot引擎复刻2048
## 前言
大一学生做着玩的，做得很烂请见谅。
GitHub账号：Vivanto-Libera，邮箱：1425078256@qq.com，GitHub仓库：https://github.com/Vivanto-Libera/2048
## 展示
<img width="650" height="724" alt="image" src="https://github.com/user-attachments/assets/6e156cb3-f1cf-46b3-ae41-add843612f49" />
就这样，很简陋。
## 实现

### 格子

我将每个格子做成了一个继承于Label的类，有一个属性number，用于表示格子上的数字。有一个类方法setNumber用于改数字：

```c#
public void setNumber(int newNumber) 
	{
		number = newNumber;
		setColor(newNumber);
		setText(newNumber);
	}
```

其中setColor用于改背景颜色：

```c#
private void setColor(int newNumber) 
	{
		int bit = 31 - BitOperations.LeadingZeroCount((uint)newNumber);
		StyleBoxFlat aStyleBox = (StyleBoxFlat)GetThemeStylebox("normal").Duplicate();
		float GandB = 1 - (float)bit / 16;
		aStyleBox.BgColor = new Color(1, GandB, GandB);
		AddThemeStyleboxOverride("normal", aStyleBox);
	}
```

setText用于设置显示的数字：

```c#
private void setText(int num) 
	{
		if(num != 0)

			{
			Text = num.ToString();
		}
		else
		{
			Text = "";
		}
	}
```

### 算法

其中只有一个比较重要的算法，就是对列的移动和合并，先看代码：

```c#
private int[] MoveLine(int a, int b, int c, int d) 
	{
		if (d == 0)
		{
			if (c == 0)
			{
				if (b == 0)
				{
					if (a != 0)
					{
						d = a;
						a = 0;
					}
				}
				else
				{
					d = b;
					c = a;
					a = 0;
					b = 0;
				}
			}
			else
			{
				d = c;
				c = b;
				b = a;
				a = 0;
			}
		}
		if (c == 0)
		{
			if (b == 0)
			{
				if (a != 0)
				{
					c = a;
					a = 0;
				}
			}
			else
			{
				c = b;
				b = a;
				a = 0;
			}
		}
		if (b == 0)
		{
			if (a != 0)
			{
				b = a;
				a = 0;
			}
		}
		if (d == c && d != 0)
		{
			d = c << 1;
			emptyTileNumber++;
			EmitSignal(SignalName.Merged, d);
			if (a == b && a != 0)
			{
				c = a << 1;
				a = 0;
				b = 0;
				emptyTileNumber++;
				EmitSignal(SignalName.Merged, c);
			}
			else
			{
				c = b;
				b = a;
				a = 0;
			}
		}
		else if (c == b && c != 0)
		{
			c = b << 1;
			b = a;
			a = 0;
			emptyTileNumber++;
			EmitSignal(SignalName.Merged, c);
		}
		else if (b == a && b != 0)
		{
			b = a << 1;
			a = 0;
			emptyTileNumber++;
			EmitSignal(SignalName.Merged, b);
		}
		int[] newLine = { a, b, c, d };
		return newLine;
	}
```

用白话来说就是：

a，b，c，d表示一列（一行）四个格子，从a向d的方向移动合并。

首先判断d是否为空，如果为空则判断c是否为空，如果c不为空则移动c到d，b到c，a到b。如果c也为空，则判断b是否为空，如不为空则移动a，b到c，d。如b为空则移动a到d（不管a是什么，移动过去无影响。

再判断此时c是否为空，步骤和上次操作类似。

之后判断此时b是否为空，如为空则将a移动到b。

经过三轮移动后，此时数字都挨到了一起，方便判断相同并进行合并了。
