<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
<title>Linux/drivers/watchdog/it8712f_wdt.c - Linux Cross Reference - Free Electrons</title>
<link rel="stylesheet" href="/style.css" type="text/css" media="screen" />
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<meta description="Linux Cross-Reference (LXR) service by Free Electrons. The easiest way to study Linux kernel sources. Available for all recent releases." />
<base href="http://lxr.free-electrons.com/"/>
</head>

<body>

<div id="wrapper">
<div id="wrapper2">
<div id="header">

	<div id="logo">
		<h1>Linux Cross Reference</h1>
		<h2><a href="http://free-electrons.com">Free Electrons</a></h2>
		<h2>Embedded Linux Experts</h2>
		<p>
		 &nbsp;&bull;&nbsp;<b><i>source navigation</i></b> &nbsp;&bull;&nbsp;<a href="diff/drivers/watchdog/it8712f_wdt.c">diff markup</a> &nbsp;&bull;&nbsp;<a href="ident">identifier search</a> &nbsp;&bull;&nbsp;<a href="search">freetext search</a> &nbsp;&bull;&nbsp;
		</p>
	</div>
</div>

<div id="topbar">
  
  <p>Version: &nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=2.0.40">2.0.40</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=2.2.26">2.2.26</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=2.4.37">2.4.37</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.12">3.12</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.13">3.13</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.14">3.14</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.15">3.15</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.16">3.16</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.17">3.17</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.18">3.18</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=3.19">3.19</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.0">4.0</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.1">4.1</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.2">4.2</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.3">4.3</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.4">4.4</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.5">4.5</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.6">4.6</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.7">4.7</a>&nbsp;<a href="source/drivers/watchdog/it8712f_wdt.c?v=4.8">4.8</a>&nbsp;<b><i>4.9</i></b></p>
  
</div>

<h1><a href="source/">Linux</a>/<a href="source/drivers/">drivers</a>/<a href="source/drivers/watchdog/">watchdog</a>/<a href="source/drivers/watchdog/it8712f_wdt.c">it8712f_wdt.c</a></h1>
<div id="lxrcode"><pre>  <a name="L1" href="source/drivers/watchdog/it8712f_wdt.c#L1">1</a> <b><i>/*</i></b>
  <a name="L2" href="source/drivers/watchdog/it8712f_wdt.c#L2">2</a> <b><i> *      IT8712F "Smart Guardian" Watchdog support</i></b>
  <a name="L3" href="source/drivers/watchdog/it8712f_wdt.c#L3">3</a> <b><i> *</i></b>
  <a name="L4" href="source/drivers/watchdog/it8712f_wdt.c#L4">4</a> <b><i> *      Copyright (c) 2006-2007 Jorge Boncompte - DTI2 &lt;jorge@dti2.net&gt;</i></b>
  <a name="L5" href="source/drivers/watchdog/it8712f_wdt.c#L5">5</a> <b><i> *</i></b>
  <a name="L6" href="source/drivers/watchdog/it8712f_wdt.c#L6">6</a> <b><i> *      Based on info and code taken from:</i></b>
  <a name="L7" href="source/drivers/watchdog/it8712f_wdt.c#L7">7</a> <b><i> *</i></b>
  <a name="L8" href="source/drivers/watchdog/it8712f_wdt.c#L8">8</a> <b><i> *      drivers/char/watchdog/scx200_wdt.c</i></b>
  <a name="L9" href="source/drivers/watchdog/it8712f_wdt.c#L9">9</a> <b><i> *      drivers/hwmon/it87.c</i></b>
 <a name="L10" href="source/drivers/watchdog/it8712f_wdt.c#L10">10</a> <b><i> *      IT8712F EC-LPC I/O Preliminary Specification 0.8.2</i></b>
 <a name="L11" href="source/drivers/watchdog/it8712f_wdt.c#L11">11</a> <b><i> *      IT8712F EC-LPC I/O Preliminary Specification 0.9.3</i></b>
 <a name="L12" href="source/drivers/watchdog/it8712f_wdt.c#L12">12</a> <b><i> *</i></b>
 <a name="L13" href="source/drivers/watchdog/it8712f_wdt.c#L13">13</a> <b><i> *      This program is free software; you can redistribute it and/or</i></b>
 <a name="L14" href="source/drivers/watchdog/it8712f_wdt.c#L14">14</a> <b><i> *      modify it under the terms of the GNU General Public License as</i></b>
 <a name="L15" href="source/drivers/watchdog/it8712f_wdt.c#L15">15</a> <b><i> *      published by the Free Software Foundation; either version 2 of the</i></b>
 <a name="L16" href="source/drivers/watchdog/it8712f_wdt.c#L16">16</a> <b><i> *      License, or (at your option) any later version.</i></b>
 <a name="L17" href="source/drivers/watchdog/it8712f_wdt.c#L17">17</a> <b><i> *</i></b>
 <a name="L18" href="source/drivers/watchdog/it8712f_wdt.c#L18">18</a> <b><i> *      The author(s) of this software shall not be held liable for damages</i></b>
 <a name="L19" href="source/drivers/watchdog/it8712f_wdt.c#L19">19</a> <b><i> *      of any nature resulting due to the use of this software. This</i></b>
 <a name="L20" href="source/drivers/watchdog/it8712f_wdt.c#L20">20</a> <b><i> *      software is provided AS-IS with no warranties.</i></b>
 <a name="L21" href="source/drivers/watchdog/it8712f_wdt.c#L21">21</a> <b><i> */</i></b>
 <a name="L22" href="source/drivers/watchdog/it8712f_wdt.c#L22">22</a> 
 <a name="L23" href="source/drivers/watchdog/it8712f_wdt.c#L23">23</a> #define <a href="ident?i=pr_fmt">pr_fmt</a>(<a href="ident?i=fmt">fmt</a>) <a href="ident?i=KBUILD_MODNAME">KBUILD_MODNAME</a> <i>": "</i> <a href="ident?i=fmt">fmt</a>
 <a name="L24" href="source/drivers/watchdog/it8712f_wdt.c#L24">24</a> 
 <a name="L25" href="source/drivers/watchdog/it8712f_wdt.c#L25">25</a> #include &lt;linux/module.h&gt;
 <a name="L26" href="source/drivers/watchdog/it8712f_wdt.c#L26">26</a> #include &lt;linux/moduleparam.h&gt;
 <a name="L27" href="source/drivers/watchdog/it8712f_wdt.c#L27">27</a> #include &lt;linux/init.h&gt;
 <a name="L28" href="source/drivers/watchdog/it8712f_wdt.c#L28">28</a> #include &lt;linux/miscdevice.h&gt;
 <a name="L29" href="source/drivers/watchdog/it8712f_wdt.c#L29">29</a> #include &lt;linux/watchdog.h&gt;
 <a name="L30" href="source/drivers/watchdog/it8712f_wdt.c#L30">30</a> #include &lt;linux/notifier.h&gt;
 <a name="L31" href="source/drivers/watchdog/it8712f_wdt.c#L31">31</a> #include &lt;linux/reboot.h&gt;
 <a name="L32" href="source/drivers/watchdog/it8712f_wdt.c#L32">32</a> #include &lt;linux/fs.h&gt;
 <a name="L33" href="source/drivers/watchdog/it8712f_wdt.c#L33">33</a> #include &lt;linux/spinlock.h&gt;
 <a name="L34" href="source/drivers/watchdog/it8712f_wdt.c#L34">34</a> #include &lt;linux/uaccess.h&gt;
 <a name="L35" href="source/drivers/watchdog/it8712f_wdt.c#L35">35</a> #include &lt;linux/io.h&gt;
 <a name="L36" href="source/drivers/watchdog/it8712f_wdt.c#L36">36</a> #include &lt;linux/ioport.h&gt;
 <a name="L37" href="source/drivers/watchdog/it8712f_wdt.c#L37">37</a> 
 <a name="L38" href="source/drivers/watchdog/it8712f_wdt.c#L38">38</a> #define <a href="ident?i=DEBUG">DEBUG</a>
 <a name="L39" href="source/drivers/watchdog/it8712f_wdt.c#L39">39</a> #define <a href="ident?i=NAME">NAME</a> <i>"it8712f_wdt"</i>
 <a name="L40" href="source/drivers/watchdog/it8712f_wdt.c#L40">40</a> 
 <a name="L41" href="source/drivers/watchdog/it8712f_wdt.c#L41">41</a> <a href="ident?i=MODULE_AUTHOR">MODULE_AUTHOR</a>(<i>"Jorge Boncompte - DTI2 &lt;jorge@dti2.net&gt;"</i>);
 <a name="L42" href="source/drivers/watchdog/it8712f_wdt.c#L42">42</a> <a href="ident?i=MODULE_DESCRIPTION">MODULE_DESCRIPTION</a>(<i>"IT8712F Watchdog Driver"</i>);
 <a name="L43" href="source/drivers/watchdog/it8712f_wdt.c#L43">43</a> <a href="ident?i=MODULE_LICENSE">MODULE_LICENSE</a>(<i>"GPL"</i>);
 <a name="L44" href="source/drivers/watchdog/it8712f_wdt.c#L44">44</a> 
 <a name="L45" href="source/drivers/watchdog/it8712f_wdt.c#L45">45</a> static int <a href="ident?i=max_units">max_units</a> = 255;
 <a name="L46" href="source/drivers/watchdog/it8712f_wdt.c#L46">46</a> static int <a href="ident?i=margin">margin</a> = 60;         <b><i>/* in seconds */</i></b>
 <a name="L47" href="source/drivers/watchdog/it8712f_wdt.c#L47">47</a> <a href="ident?i=module_param">module_param</a>(<a href="ident?i=margin">margin</a>, int, 0);
 <a name="L48" href="source/drivers/watchdog/it8712f_wdt.c#L48">48</a> <a href="ident?i=MODULE_PARM_DESC">MODULE_PARM_DESC</a>(<a href="ident?i=margin">margin</a>, <i>"Watchdog margin in seconds"</i>);
 <a name="L49" href="source/drivers/watchdog/it8712f_wdt.c#L49">49</a> 
 <a name="L50" href="source/drivers/watchdog/it8712f_wdt.c#L50">50</a> static <a href="ident?i=bool">bool</a> <a href="ident?i=nowayout">nowayout</a> = <a href="ident?i=WATCHDOG_NOWAYOUT">WATCHDOG_NOWAYOUT</a>;
 <a name="L51" href="source/drivers/watchdog/it8712f_wdt.c#L51">51</a> <a href="ident?i=module_param">module_param</a>(<a href="ident?i=nowayout">nowayout</a>, <a href="ident?i=bool">bool</a>, 0);
 <a name="L52" href="source/drivers/watchdog/it8712f_wdt.c#L52">52</a> <a href="ident?i=MODULE_PARM_DESC">MODULE_PARM_DESC</a>(<a href="ident?i=nowayout">nowayout</a>, <i>"Disable watchdog shutdown on close"</i>);
 <a name="L53" href="source/drivers/watchdog/it8712f_wdt.c#L53">53</a> 
 <a name="L54" href="source/drivers/watchdog/it8712f_wdt.c#L54">54</a> static unsigned long <a href="ident?i=wdt_open">wdt_open</a>;
 <a name="L55" href="source/drivers/watchdog/it8712f_wdt.c#L55">55</a> static unsigned <a href="ident?i=expect_close">expect_close</a>;
 <a name="L56" href="source/drivers/watchdog/it8712f_wdt.c#L56">56</a> static unsigned char <a href="ident?i=revision">revision</a>;
 <a name="L57" href="source/drivers/watchdog/it8712f_wdt.c#L57">57</a> 
 <a name="L58" href="source/drivers/watchdog/it8712f_wdt.c#L58">58</a> <b><i>/* Dog Food address - We use the game port address */</i></b>
 <a name="L59" href="source/drivers/watchdog/it8712f_wdt.c#L59">59</a> static unsigned short <a href="ident?i=address">address</a>;
 <a name="L60" href="source/drivers/watchdog/it8712f_wdt.c#L60">60</a> 
 <a name="L61" href="source/drivers/watchdog/it8712f_wdt.c#L61">61</a> #define <a href="ident?i=REG">REG</a>             0x2e    <b><i>/* The register to read/write */</i></b>
 <a name="L62" href="source/drivers/watchdog/it8712f_wdt.c#L62">62</a> #define <a href="ident?i=VAL">VAL</a>             0x2f    <b><i>/* The value to read/write */</i></b>
 <a name="L63" href="source/drivers/watchdog/it8712f_wdt.c#L63">63</a> 
 <a name="L64" href="source/drivers/watchdog/it8712f_wdt.c#L64">64</a> #define <a href="ident?i=LDN">LDN</a>             0x07    <b><i>/* Register: Logical device select */</i></b>
 <a name="L65" href="source/drivers/watchdog/it8712f_wdt.c#L65">65</a> #define <a href="ident?i=DEVID">DEVID</a>           0x20    <b><i>/* Register: Device ID */</i></b>
 <a name="L66" href="source/drivers/watchdog/it8712f_wdt.c#L66">66</a> #define <a href="ident?i=DEVREV">DEVREV</a>          0x22    <b><i>/* Register: Device Revision */</i></b>
 <a name="L67" href="source/drivers/watchdog/it8712f_wdt.c#L67">67</a> #define <a href="ident?i=ACT_REG">ACT_REG</a>         0x30    <b><i>/* LDN Register: Activation */</i></b>
 <a name="L68" href="source/drivers/watchdog/it8712f_wdt.c#L68">68</a> #define <a href="ident?i=BASE_REG">BASE_REG</a>        0x60    <b><i>/* LDN Register: Base address */</i></b>
 <a name="L69" href="source/drivers/watchdog/it8712f_wdt.c#L69">69</a> 
 <a name="L70" href="source/drivers/watchdog/it8712f_wdt.c#L70">70</a> #define <a href="ident?i=IT8712F_DEVID">IT8712F_DEVID</a>   0x8712
 <a name="L71" href="source/drivers/watchdog/it8712f_wdt.c#L71">71</a> 
 <a name="L72" href="source/drivers/watchdog/it8712f_wdt.c#L72">72</a> #define <a href="ident?i=LDN_GPIO">LDN_GPIO</a>        0x07    <b><i>/* GPIO and Watch Dog Timer */</i></b>
 <a name="L73" href="source/drivers/watchdog/it8712f_wdt.c#L73">73</a> #define <a href="ident?i=LDN_GAME">LDN_GAME</a>        0x09    <b><i>/* Game Port */</i></b>
 <a name="L74" href="source/drivers/watchdog/it8712f_wdt.c#L74">74</a> 
 <a name="L75" href="source/drivers/watchdog/it8712f_wdt.c#L75">75</a> #define <a href="ident?i=WDT_CONTROL">WDT_CONTROL</a>     0x71    <b><i>/* WDT Register: Control */</i></b>
 <a name="L76" href="source/drivers/watchdog/it8712f_wdt.c#L76">76</a> #define <a href="ident?i=WDT_CONFIG">WDT_CONFIG</a>      0x72    <b><i>/* WDT Register: Configuration */</i></b>
 <a name="L77" href="source/drivers/watchdog/it8712f_wdt.c#L77">77</a> #define <a href="ident?i=WDT_TIMEOUT">WDT_TIMEOUT</a>     0x73    <b><i>/* WDT Register: Timeout Value */</i></b>
 <a name="L78" href="source/drivers/watchdog/it8712f_wdt.c#L78">78</a> 
 <a name="L79" href="source/drivers/watchdog/it8712f_wdt.c#L79">79</a> #define <a href="ident?i=WDT_RESET_GAME">WDT_RESET_GAME</a>  0x10    <b><i>/* Reset timer on read or write to game port */</i></b>
 <a name="L80" href="source/drivers/watchdog/it8712f_wdt.c#L80">80</a> #define <a href="ident?i=WDT_RESET_KBD">WDT_RESET_KBD</a>   0x20    <b><i>/* Reset timer on keyboard interrupt */</i></b>
 <a name="L81" href="source/drivers/watchdog/it8712f_wdt.c#L81">81</a> #define <a href="ident?i=WDT_RESET_MOUSE">WDT_RESET_MOUSE</a> 0x40    <b><i>/* Reset timer on mouse interrupt */</i></b>
 <a name="L82" href="source/drivers/watchdog/it8712f_wdt.c#L82">82</a> #define <a href="ident?i=WDT_RESET_CIR">WDT_RESET_CIR</a>   0x80    <b><i>/* Reset timer on consumer IR interrupt */</i></b>
 <a name="L83" href="source/drivers/watchdog/it8712f_wdt.c#L83">83</a> 
 <a name="L84" href="source/drivers/watchdog/it8712f_wdt.c#L84">84</a> #define <a href="ident?i=WDT_UNIT_SEC">WDT_UNIT_SEC</a>    0x80    <b><i>/* If 0 in MINUTES */</i></b>
 <a name="L85" href="source/drivers/watchdog/it8712f_wdt.c#L85">85</a> 
 <a name="L86" href="source/drivers/watchdog/it8712f_wdt.c#L86">86</a> #define <a href="ident?i=WDT_OUT_PWROK">WDT_OUT_PWROK</a>   0x10    <b><i>/* Pulse PWROK on timeout */</i></b>
 <a name="L87" href="source/drivers/watchdog/it8712f_wdt.c#L87">87</a> #define <a href="ident?i=WDT_OUT_KRST">WDT_OUT_KRST</a>    0x40    <b><i>/* Pulse reset on timeout */</i></b>
 <a name="L88" href="source/drivers/watchdog/it8712f_wdt.c#L88">88</a> 
 <a name="L89" href="source/drivers/watchdog/it8712f_wdt.c#L89">89</a> static int <a href="ident?i=wdt_control_reg">wdt_control_reg</a> = <a href="ident?i=WDT_RESET_GAME">WDT_RESET_GAME</a>;
 <a name="L90" href="source/drivers/watchdog/it8712f_wdt.c#L90">90</a> <a href="ident?i=module_param">module_param</a>(<a href="ident?i=wdt_control_reg">wdt_control_reg</a>, int, 0);
 <a name="L91" href="source/drivers/watchdog/it8712f_wdt.c#L91">91</a> <a href="ident?i=MODULE_PARM_DESC">MODULE_PARM_DESC</a>(<a href="ident?i=wdt_control_reg">wdt_control_reg</a>, <i>"Value to write to watchdog control "</i>
 <a name="L92" href="source/drivers/watchdog/it8712f_wdt.c#L92">92</a>                 <i>"register. The default WDT_RESET_GAME resets the timer on "</i>
 <a name="L93" href="source/drivers/watchdog/it8712f_wdt.c#L93">93</a>                 <i>"game port reads that this driver generates. You can also "</i>
 <a name="L94" href="source/drivers/watchdog/it8712f_wdt.c#L94">94</a>                 <i>"use KBD, MOUSE or CIR if you have some external way to "</i>
 <a name="L95" href="source/drivers/watchdog/it8712f_wdt.c#L95">95</a>                 <i>"generate those interrupts."</i>);
 <a name="L96" href="source/drivers/watchdog/it8712f_wdt.c#L96">96</a> 
 <a name="L97" href="source/drivers/watchdog/it8712f_wdt.c#L97">97</a> static int <a href="ident?i=superio_inb">superio_inb</a>(int <a href="ident?i=reg">reg</a>)
 <a name="L98" href="source/drivers/watchdog/it8712f_wdt.c#L98">98</a> {
 <a name="L99" href="source/drivers/watchdog/it8712f_wdt.c#L99">99</a>         <a href="ident?i=outb">outb</a>(<a href="ident?i=reg">reg</a>, <a href="ident?i=REG">REG</a>);
<a name="L100" href="source/drivers/watchdog/it8712f_wdt.c#L100">100</a>         return <a href="ident?i=inb">inb</a>(<a href="ident?i=VAL">VAL</a>);
<a name="L101" href="source/drivers/watchdog/it8712f_wdt.c#L101">101</a> }
<a name="L102" href="source/drivers/watchdog/it8712f_wdt.c#L102">102</a> 
<a name="L103" href="source/drivers/watchdog/it8712f_wdt.c#L103">103</a> static void <a href="ident?i=superio_outb">superio_outb</a>(int <a href="ident?i=val">val</a>, int <a href="ident?i=reg">reg</a>)
<a name="L104" href="source/drivers/watchdog/it8712f_wdt.c#L104">104</a> {
<a name="L105" href="source/drivers/watchdog/it8712f_wdt.c#L105">105</a>         <a href="ident?i=outb">outb</a>(<a href="ident?i=reg">reg</a>, <a href="ident?i=REG">REG</a>);
<a name="L106" href="source/drivers/watchdog/it8712f_wdt.c#L106">106</a>         <a href="ident?i=outb">outb</a>(<a href="ident?i=val">val</a>, <a href="ident?i=VAL">VAL</a>);
<a name="L107" href="source/drivers/watchdog/it8712f_wdt.c#L107">107</a> }
<a name="L108" href="source/drivers/watchdog/it8712f_wdt.c#L108">108</a> 
<a name="L109" href="source/drivers/watchdog/it8712f_wdt.c#L109">109</a> static int <a href="ident?i=superio_inw">superio_inw</a>(int <a href="ident?i=reg">reg</a>)
<a name="L110" href="source/drivers/watchdog/it8712f_wdt.c#L110">110</a> {
<a name="L111" href="source/drivers/watchdog/it8712f_wdt.c#L111">111</a>         int <a href="ident?i=val">val</a>;
<a name="L112" href="source/drivers/watchdog/it8712f_wdt.c#L112">112</a>         <a href="ident?i=outb">outb</a>(<a href="ident?i=reg">reg</a>++, <a href="ident?i=REG">REG</a>);
<a name="L113" href="source/drivers/watchdog/it8712f_wdt.c#L113">113</a>         <a href="ident?i=val">val</a> = <a href="ident?i=inb">inb</a>(<a href="ident?i=VAL">VAL</a>) &lt;&lt; 8;
<a name="L114" href="source/drivers/watchdog/it8712f_wdt.c#L114">114</a>         <a href="ident?i=outb">outb</a>(<a href="ident?i=reg">reg</a>, <a href="ident?i=REG">REG</a>);
<a name="L115" href="source/drivers/watchdog/it8712f_wdt.c#L115">115</a>         <a href="ident?i=val">val</a> |= <a href="ident?i=inb">inb</a>(<a href="ident?i=VAL">VAL</a>);
<a name="L116" href="source/drivers/watchdog/it8712f_wdt.c#L116">116</a>         return <a href="ident?i=val">val</a>;
<a name="L117" href="source/drivers/watchdog/it8712f_wdt.c#L117">117</a> }
<a name="L118" href="source/drivers/watchdog/it8712f_wdt.c#L118">118</a> 
<a name="L119" href="source/drivers/watchdog/it8712f_wdt.c#L119">119</a> static inline void <a href="ident?i=superio_select">superio_select</a>(int ldn)
<a name="L120" href="source/drivers/watchdog/it8712f_wdt.c#L120">120</a> {
<a name="L121" href="source/drivers/watchdog/it8712f_wdt.c#L121">121</a>         <a href="ident?i=outb">outb</a>(<a href="ident?i=LDN">LDN</a>, <a href="ident?i=REG">REG</a>);
<a name="L122" href="source/drivers/watchdog/it8712f_wdt.c#L122">122</a>         <a href="ident?i=outb">outb</a>(ldn, <a href="ident?i=VAL">VAL</a>);
<a name="L123" href="source/drivers/watchdog/it8712f_wdt.c#L123">123</a> }
<a name="L124" href="source/drivers/watchdog/it8712f_wdt.c#L124">124</a> 
<a name="L125" href="source/drivers/watchdog/it8712f_wdt.c#L125">125</a> static inline int <a href="ident?i=superio_enter">superio_enter</a>(void)
<a name="L126" href="source/drivers/watchdog/it8712f_wdt.c#L126">126</a> {
<a name="L127" href="source/drivers/watchdog/it8712f_wdt.c#L127">127</a>         <b><i>/*</i></b>
<a name="L128" href="source/drivers/watchdog/it8712f_wdt.c#L128">128</a> <b><i>         * Try to reserve REG and REG + 1 for exclusive access.</i></b>
<a name="L129" href="source/drivers/watchdog/it8712f_wdt.c#L129">129</a> <b><i>         */</i></b>
<a name="L130" href="source/drivers/watchdog/it8712f_wdt.c#L130">130</a>         if (!<a href="ident?i=request_muxed_region">request_muxed_region</a>(<a href="ident?i=REG">REG</a>, 2, <a href="ident?i=NAME">NAME</a>))
<a name="L131" href="source/drivers/watchdog/it8712f_wdt.c#L131">131</a>                 return -<a href="ident?i=EBUSY">EBUSY</a>;
<a name="L132" href="source/drivers/watchdog/it8712f_wdt.c#L132">132</a> 
<a name="L133" href="source/drivers/watchdog/it8712f_wdt.c#L133">133</a>         <a href="ident?i=outb">outb</a>(0x87, <a href="ident?i=REG">REG</a>);
<a name="L134" href="source/drivers/watchdog/it8712f_wdt.c#L134">134</a>         <a href="ident?i=outb">outb</a>(0x01, <a href="ident?i=REG">REG</a>);
<a name="L135" href="source/drivers/watchdog/it8712f_wdt.c#L135">135</a>         <a href="ident?i=outb">outb</a>(0x55, <a href="ident?i=REG">REG</a>);
<a name="L136" href="source/drivers/watchdog/it8712f_wdt.c#L136">136</a>         <a href="ident?i=outb">outb</a>(0x55, <a href="ident?i=REG">REG</a>);
<a name="L137" href="source/drivers/watchdog/it8712f_wdt.c#L137">137</a>         return 0;
<a name="L138" href="source/drivers/watchdog/it8712f_wdt.c#L138">138</a> }
<a name="L139" href="source/drivers/watchdog/it8712f_wdt.c#L139">139</a> 
<a name="L140" href="source/drivers/watchdog/it8712f_wdt.c#L140">140</a> static inline void <a href="ident?i=superio_exit">superio_exit</a>(void)
<a name="L141" href="source/drivers/watchdog/it8712f_wdt.c#L141">141</a> {
<a name="L142" href="source/drivers/watchdog/it8712f_wdt.c#L142">142</a>         <a href="ident?i=outb">outb</a>(0x02, <a href="ident?i=REG">REG</a>);
<a name="L143" href="source/drivers/watchdog/it8712f_wdt.c#L143">143</a>         <a href="ident?i=outb">outb</a>(0x02, <a href="ident?i=VAL">VAL</a>);
<a name="L144" href="source/drivers/watchdog/it8712f_wdt.c#L144">144</a>         <a href="ident?i=release_region">release_region</a>(<a href="ident?i=REG">REG</a>, 2);
<a name="L145" href="source/drivers/watchdog/it8712f_wdt.c#L145">145</a> }
<a name="L146" href="source/drivers/watchdog/it8712f_wdt.c#L146">146</a> 
<a name="L147" href="source/drivers/watchdog/it8712f_wdt.c#L147">147</a> static inline void <a href="ident?i=it8712f_wdt_ping">it8712f_wdt_ping</a>(void)
<a name="L148" href="source/drivers/watchdog/it8712f_wdt.c#L148">148</a> {
<a name="L149" href="source/drivers/watchdog/it8712f_wdt.c#L149">149</a>         if (<a href="ident?i=wdt_control_reg">wdt_control_reg</a> &amp; <a href="ident?i=WDT_RESET_GAME">WDT_RESET_GAME</a>)
<a name="L150" href="source/drivers/watchdog/it8712f_wdt.c#L150">150</a>                 <a href="ident?i=inb">inb</a>(<a href="ident?i=address">address</a>);
<a name="L151" href="source/drivers/watchdog/it8712f_wdt.c#L151">151</a> }
<a name="L152" href="source/drivers/watchdog/it8712f_wdt.c#L152">152</a> 
<a name="L153" href="source/drivers/watchdog/it8712f_wdt.c#L153">153</a> static void <a href="ident?i=it8712f_wdt_update_margin">it8712f_wdt_update_margin</a>(void)
<a name="L154" href="source/drivers/watchdog/it8712f_wdt.c#L154">154</a> {
<a name="L155" href="source/drivers/watchdog/it8712f_wdt.c#L155">155</a>         int <a href="ident?i=config">config</a> = <a href="ident?i=WDT_OUT_KRST">WDT_OUT_KRST</a> | <a href="ident?i=WDT_OUT_PWROK">WDT_OUT_PWROK</a>;
<a name="L156" href="source/drivers/watchdog/it8712f_wdt.c#L156">156</a>         int <a href="ident?i=units">units</a> = <a href="ident?i=margin">margin</a>;
<a name="L157" href="source/drivers/watchdog/it8712f_wdt.c#L157">157</a> 
<a name="L158" href="source/drivers/watchdog/it8712f_wdt.c#L158">158</a>         <b><i>/* Switch to minutes precision if the configured margin</i></b>
<a name="L159" href="source/drivers/watchdog/it8712f_wdt.c#L159">159</a> <b><i>         * value does not fit within the register width.</i></b>
<a name="L160" href="source/drivers/watchdog/it8712f_wdt.c#L160">160</a> <b><i>         */</i></b>
<a name="L161" href="source/drivers/watchdog/it8712f_wdt.c#L161">161</a>         if (<a href="ident?i=units">units</a> &lt;= <a href="ident?i=max_units">max_units</a>) {
<a name="L162" href="source/drivers/watchdog/it8712f_wdt.c#L162">162</a>                 <a href="ident?i=config">config</a> |= <a href="ident?i=WDT_UNIT_SEC">WDT_UNIT_SEC</a>; <b><i>/* else UNIT is MINUTES */</i></b>
<a name="L163" href="source/drivers/watchdog/it8712f_wdt.c#L163">163</a>                 <a href="ident?i=pr_info">pr_info</a>(<i>"timer margin %d seconds\n"</i>, <a href="ident?i=units">units</a>);
<a name="L164" href="source/drivers/watchdog/it8712f_wdt.c#L164">164</a>         } else {
<a name="L165" href="source/drivers/watchdog/it8712f_wdt.c#L165">165</a>                 <a href="ident?i=units">units</a> /= 60;
<a name="L166" href="source/drivers/watchdog/it8712f_wdt.c#L166">166</a>                 <a href="ident?i=pr_info">pr_info</a>(<i>"timer margin %d minutes\n"</i>, <a href="ident?i=units">units</a>);
<a name="L167" href="source/drivers/watchdog/it8712f_wdt.c#L167">167</a>         }
<a name="L168" href="source/drivers/watchdog/it8712f_wdt.c#L168">168</a>         <a href="ident?i=superio_outb">superio_outb</a>(<a href="ident?i=config">config</a>, <a href="ident?i=WDT_CONFIG">WDT_CONFIG</a>);
<a name="L169" href="source/drivers/watchdog/it8712f_wdt.c#L169">169</a> 
<a name="L170" href="source/drivers/watchdog/it8712f_wdt.c#L170">170</a>         if (<a href="ident?i=revision">revision</a> &gt;= 0x08)
<a name="L171" href="source/drivers/watchdog/it8712f_wdt.c#L171">171</a>                 <a href="ident?i=superio_outb">superio_outb</a>(<a href="ident?i=units">units</a> &gt;&gt; 8, <a href="ident?i=WDT_TIMEOUT">WDT_TIMEOUT</a> + 1);
<a name="L172" href="source/drivers/watchdog/it8712f_wdt.c#L172">172</a>         <a href="ident?i=superio_outb">superio_outb</a>(<a href="ident?i=units">units</a>, <a href="ident?i=WDT_TIMEOUT">WDT_TIMEOUT</a>);
<a name="L173" href="source/drivers/watchdog/it8712f_wdt.c#L173">173</a> }
<a name="L174" href="source/drivers/watchdog/it8712f_wdt.c#L174">174</a> 
<a name="L175" href="source/drivers/watchdog/it8712f_wdt.c#L175">175</a> static int <a href="ident?i=it8712f_wdt_get_status">it8712f_wdt_get_status</a>(void)
<a name="L176" href="source/drivers/watchdog/it8712f_wdt.c#L176">176</a> {
<a name="L177" href="source/drivers/watchdog/it8712f_wdt.c#L177">177</a>         if (<a href="ident?i=superio_inb">superio_inb</a>(<a href="ident?i=WDT_CONTROL">WDT_CONTROL</a>) &amp; 0x01)
<a name="L178" href="source/drivers/watchdog/it8712f_wdt.c#L178">178</a>                 return <a href="ident?i=WDIOF_CARDRESET">WDIOF_CARDRESET</a>;
<a name="L179" href="source/drivers/watchdog/it8712f_wdt.c#L179">179</a>         else
<a name="L180" href="source/drivers/watchdog/it8712f_wdt.c#L180">180</a>                 return 0;
<a name="L181" href="source/drivers/watchdog/it8712f_wdt.c#L181">181</a> }
<a name="L182" href="source/drivers/watchdog/it8712f_wdt.c#L182">182</a> 
<a name="L183" href="source/drivers/watchdog/it8712f_wdt.c#L183">183</a> static int <a href="ident?i=it8712f_wdt_enable">it8712f_wdt_enable</a>(void)
<a name="L184" href="source/drivers/watchdog/it8712f_wdt.c#L184">184</a> {
<a name="L185" href="source/drivers/watchdog/it8712f_wdt.c#L185">185</a>         int <a href="ident?i=ret">ret</a> = <a href="ident?i=superio_enter">superio_enter</a>();
<a name="L186" href="source/drivers/watchdog/it8712f_wdt.c#L186">186</a>         if (<a href="ident?i=ret">ret</a>)
<a name="L187" href="source/drivers/watchdog/it8712f_wdt.c#L187">187</a>                 return <a href="ident?i=ret">ret</a>;
<a name="L188" href="source/drivers/watchdog/it8712f_wdt.c#L188">188</a> 
<a name="L189" href="source/drivers/watchdog/it8712f_wdt.c#L189">189</a>         <a href="ident?i=pr_debug">pr_debug</a>(<i>"enabling watchdog timer\n"</i>);
<a name="L190" href="source/drivers/watchdog/it8712f_wdt.c#L190">190</a>         <a href="ident?i=superio_select">superio_select</a>(<a href="ident?i=LDN_GPIO">LDN_GPIO</a>);
<a name="L191" href="source/drivers/watchdog/it8712f_wdt.c#L191">191</a> 
<a name="L192" href="source/drivers/watchdog/it8712f_wdt.c#L192">192</a>         <a href="ident?i=superio_outb">superio_outb</a>(<a href="ident?i=wdt_control_reg">wdt_control_reg</a>, <a href="ident?i=WDT_CONTROL">WDT_CONTROL</a>);
<a name="L193" href="source/drivers/watchdog/it8712f_wdt.c#L193">193</a> 
<a name="L194" href="source/drivers/watchdog/it8712f_wdt.c#L194">194</a>         <a href="ident?i=it8712f_wdt_update_margin">it8712f_wdt_update_margin</a>();
<a name="L195" href="source/drivers/watchdog/it8712f_wdt.c#L195">195</a> 
<a name="L196" href="source/drivers/watchdog/it8712f_wdt.c#L196">196</a>         <a href="ident?i=superio_exit">superio_exit</a>();
<a name="L197" href="source/drivers/watchdog/it8712f_wdt.c#L197">197</a> 
<a name="L198" href="source/drivers/watchdog/it8712f_wdt.c#L198">198</a>         <a href="ident?i=it8712f_wdt_ping">it8712f_wdt_ping</a>();
<a name="L199" href="source/drivers/watchdog/it8712f_wdt.c#L199">199</a> 
<a name="L200" href="source/drivers/watchdog/it8712f_wdt.c#L200">200</a>         return 0;
<a name="L201" href="source/drivers/watchdog/it8712f_wdt.c#L201">201</a> }
<a name="L202" href="source/drivers/watchdog/it8712f_wdt.c#L202">202</a> 
<a name="L203" href="source/drivers/watchdog/it8712f_wdt.c#L203">203</a> static int <a href="ident?i=it8712f_wdt_disable">it8712f_wdt_disable</a>(void)
<a name="L204" href="source/drivers/watchdog/it8712f_wdt.c#L204">204</a> {
<a name="L205" href="source/drivers/watchdog/it8712f_wdt.c#L205">205</a>         int <a href="ident?i=ret">ret</a> = <a href="ident?i=superio_enter">superio_enter</a>();
<a name="L206" href="source/drivers/watchdog/it8712f_wdt.c#L206">206</a>         if (<a href="ident?i=ret">ret</a>)
<a name="L207" href="source/drivers/watchdog/it8712f_wdt.c#L207">207</a>                 return <a href="ident?i=ret">ret</a>;
<a name="L208" href="source/drivers/watchdog/it8712f_wdt.c#L208">208</a> 
<a name="L209" href="source/drivers/watchdog/it8712f_wdt.c#L209">209</a>         <a href="ident?i=pr_debug">pr_debug</a>(<i>"disabling watchdog timer\n"</i>);
<a name="L210" href="source/drivers/watchdog/it8712f_wdt.c#L210">210</a>         <a href="ident?i=superio_select">superio_select</a>(<a href="ident?i=LDN_GPIO">LDN_GPIO</a>);
<a name="L211" href="source/drivers/watchdog/it8712f_wdt.c#L211">211</a> 
<a name="L212" href="source/drivers/watchdog/it8712f_wdt.c#L212">212</a>         <a href="ident?i=superio_outb">superio_outb</a>(0, <a href="ident?i=WDT_CONFIG">WDT_CONFIG</a>);
<a name="L213" href="source/drivers/watchdog/it8712f_wdt.c#L213">213</a>         <a href="ident?i=superio_outb">superio_outb</a>(0, <a href="ident?i=WDT_CONTROL">WDT_CONTROL</a>);
<a name="L214" href="source/drivers/watchdog/it8712f_wdt.c#L214">214</a>         if (<a href="ident?i=revision">revision</a> &gt;= 0x08)
<a name="L215" href="source/drivers/watchdog/it8712f_wdt.c#L215">215</a>                 <a href="ident?i=superio_outb">superio_outb</a>(0, <a href="ident?i=WDT_TIMEOUT">WDT_TIMEOUT</a> + 1);
<a name="L216" href="source/drivers/watchdog/it8712f_wdt.c#L216">216</a>         <a href="ident?i=superio_outb">superio_outb</a>(0, <a href="ident?i=WDT_TIMEOUT">WDT_TIMEOUT</a>);
<a name="L217" href="source/drivers/watchdog/it8712f_wdt.c#L217">217</a> 
<a name="L218" href="source/drivers/watchdog/it8712f_wdt.c#L218">218</a>         <a href="ident?i=superio_exit">superio_exit</a>();
<a name="L219" href="source/drivers/watchdog/it8712f_wdt.c#L219">219</a>         return 0;
<a name="L220" href="source/drivers/watchdog/it8712f_wdt.c#L220">220</a> }
<a name="L221" href="source/drivers/watchdog/it8712f_wdt.c#L221">221</a> 
<a name="L222" href="source/drivers/watchdog/it8712f_wdt.c#L222">222</a> static int <a href="ident?i=it8712f_wdt_notify">it8712f_wdt_notify</a>(struct <a href="ident?i=notifier_block">notifier_block</a> *<a href="ident?i=this">this</a>,
<a name="L223" href="source/drivers/watchdog/it8712f_wdt.c#L223">223</a>                     unsigned long <a href="ident?i=code">code</a>, void *<a href="ident?i=unused">unused</a>)
<a name="L224" href="source/drivers/watchdog/it8712f_wdt.c#L224">224</a> {
<a name="L225" href="source/drivers/watchdog/it8712f_wdt.c#L225">225</a>         if (<a href="ident?i=code">code</a> == <a href="ident?i=SYS_HALT">SYS_HALT</a> || <a href="ident?i=code">code</a> == <a href="ident?i=SYS_POWER_OFF">SYS_POWER_OFF</a>)
<a name="L226" href="source/drivers/watchdog/it8712f_wdt.c#L226">226</a>                 if (!<a href="ident?i=nowayout">nowayout</a>)
<a name="L227" href="source/drivers/watchdog/it8712f_wdt.c#L227">227</a>                         <a href="ident?i=it8712f_wdt_disable">it8712f_wdt_disable</a>();
<a name="L228" href="source/drivers/watchdog/it8712f_wdt.c#L228">228</a> 
<a name="L229" href="source/drivers/watchdog/it8712f_wdt.c#L229">229</a>         return <a href="ident?i=NOTIFY_DONE">NOTIFY_DONE</a>;
<a name="L230" href="source/drivers/watchdog/it8712f_wdt.c#L230">230</a> }
<a name="L231" href="source/drivers/watchdog/it8712f_wdt.c#L231">231</a> 
<a name="L232" href="source/drivers/watchdog/it8712f_wdt.c#L232">232</a> static struct <a href="ident?i=notifier_block">notifier_block</a> <a href="ident?i=it8712f_wdt_notifier">it8712f_wdt_notifier</a> = {
<a name="L233" href="source/drivers/watchdog/it8712f_wdt.c#L233">233</a>         .notifier_call = <a href="ident?i=it8712f_wdt_notify">it8712f_wdt_notify</a>,
<a name="L234" href="source/drivers/watchdog/it8712f_wdt.c#L234">234</a> };
<a name="L235" href="source/drivers/watchdog/it8712f_wdt.c#L235">235</a> 
<a name="L236" href="source/drivers/watchdog/it8712f_wdt.c#L236">236</a> static <a href="ident?i=ssize_t">ssize_t</a> <a href="ident?i=it8712f_wdt_write">it8712f_wdt_write</a>(struct <a href="ident?i=file">file</a> *<a href="ident?i=file">file</a>, const char <a href="ident?i=__user">__user</a> *<a href="ident?i=data">data</a>,
<a name="L237" href="source/drivers/watchdog/it8712f_wdt.c#L237">237</a>                                         <a href="ident?i=size_t">size_t</a> <a href="ident?i=len">len</a>, <a href="ident?i=loff_t">loff_t</a> *<a href="ident?i=ppos">ppos</a>)
<a name="L238" href="source/drivers/watchdog/it8712f_wdt.c#L238">238</a> {
<a name="L239" href="source/drivers/watchdog/it8712f_wdt.c#L239">239</a>         <b><i>/* check for a magic close character */</i></b>
<a name="L240" href="source/drivers/watchdog/it8712f_wdt.c#L240">240</a>         if (<a href="ident?i=len">len</a>) {
<a name="L241" href="source/drivers/watchdog/it8712f_wdt.c#L241">241</a>                 <a href="ident?i=size_t">size_t</a> <a href="ident?i=i">i</a>;
<a name="L242" href="source/drivers/watchdog/it8712f_wdt.c#L242">242</a> 
<a name="L243" href="source/drivers/watchdog/it8712f_wdt.c#L243">243</a>                 <a href="ident?i=it8712f_wdt_ping">it8712f_wdt_ping</a>();
<a name="L244" href="source/drivers/watchdog/it8712f_wdt.c#L244">244</a> 
<a name="L245" href="source/drivers/watchdog/it8712f_wdt.c#L245">245</a>                 <a href="ident?i=expect_close">expect_close</a> = 0;
<a name="L246" href="source/drivers/watchdog/it8712f_wdt.c#L246">246</a>                 for (<a href="ident?i=i">i</a> = 0; <a href="ident?i=i">i</a> &lt; <a href="ident?i=len">len</a>; ++<a href="ident?i=i">i</a>) {
<a name="L247" href="source/drivers/watchdog/it8712f_wdt.c#L247">247</a>                         char <a href="ident?i=c">c</a>;
<a name="L248" href="source/drivers/watchdog/it8712f_wdt.c#L248">248</a>                         if (<a href="ident?i=get_user">get_user</a>(<a href="ident?i=c">c</a>, <a href="ident?i=data">data</a> + <a href="ident?i=i">i</a>))
<a name="L249" href="source/drivers/watchdog/it8712f_wdt.c#L249">249</a>                                 return -<a href="ident?i=EFAULT">EFAULT</a>;
<a name="L250" href="source/drivers/watchdog/it8712f_wdt.c#L250">250</a>                         if (<a href="ident?i=c">c</a> == <i>'V'</i>)
<a name="L251" href="source/drivers/watchdog/it8712f_wdt.c#L251">251</a>                                 <a href="ident?i=expect_close">expect_close</a> = 42;
<a name="L252" href="source/drivers/watchdog/it8712f_wdt.c#L252">252</a>                 }
<a name="L253" href="source/drivers/watchdog/it8712f_wdt.c#L253">253</a>         }
<a name="L254" href="source/drivers/watchdog/it8712f_wdt.c#L254">254</a> 
<a name="L255" href="source/drivers/watchdog/it8712f_wdt.c#L255">255</a>         return <a href="ident?i=len">len</a>;
<a name="L256" href="source/drivers/watchdog/it8712f_wdt.c#L256">256</a> }
<a name="L257" href="source/drivers/watchdog/it8712f_wdt.c#L257">257</a> 
<a name="L258" href="source/drivers/watchdog/it8712f_wdt.c#L258">258</a> static long <a href="ident?i=it8712f_wdt_ioctl">it8712f_wdt_ioctl</a>(struct <a href="ident?i=file">file</a> *<a href="ident?i=file">file</a>, unsigned int <a href="ident?i=cmd">cmd</a>,
<a name="L259" href="source/drivers/watchdog/it8712f_wdt.c#L259">259</a>                                                         unsigned long <a href="ident?i=arg">arg</a>)
<a name="L260" href="source/drivers/watchdog/it8712f_wdt.c#L260">260</a> {
<a name="L261" href="source/drivers/watchdog/it8712f_wdt.c#L261">261</a>         void <a href="ident?i=__user">__user</a> *<a href="ident?i=argp">argp</a> = (void <a href="ident?i=__user">__user</a> *)<a href="ident?i=arg">arg</a>;
<a name="L262" href="source/drivers/watchdog/it8712f_wdt.c#L262">262</a>         int <a href="ident?i=__user">__user</a> *<a href="ident?i=p">p</a> = <a href="ident?i=argp">argp</a>;
<a name="L263" href="source/drivers/watchdog/it8712f_wdt.c#L263">263</a>         static const struct <a href="ident?i=watchdog_info">watchdog_info</a> <a href="ident?i=ident">ident</a> = {
<a name="L264" href="source/drivers/watchdog/it8712f_wdt.c#L264">264</a>                 .<a href="ident?i=identity">identity</a> = <i>"IT8712F Watchdog"</i>,
<a name="L265" href="source/drivers/watchdog/it8712f_wdt.c#L265">265</a>                 .<a href="ident?i=firmware_version">firmware_version</a> = 1,
<a name="L266" href="source/drivers/watchdog/it8712f_wdt.c#L266">266</a>                 .<a href="ident?i=options">options</a> = <a href="ident?i=WDIOF_SETTIMEOUT">WDIOF_SETTIMEOUT</a> | <a href="ident?i=WDIOF_KEEPALIVEPING">WDIOF_KEEPALIVEPING</a> |
<a name="L267" href="source/drivers/watchdog/it8712f_wdt.c#L267">267</a>                                                 <a href="ident?i=WDIOF_MAGICCLOSE">WDIOF_MAGICCLOSE</a>,
<a name="L268" href="source/drivers/watchdog/it8712f_wdt.c#L268">268</a>         };
<a name="L269" href="source/drivers/watchdog/it8712f_wdt.c#L269">269</a>         int <a href="ident?i=value">value</a>;
<a name="L270" href="source/drivers/watchdog/it8712f_wdt.c#L270">270</a>         int <a href="ident?i=ret">ret</a>;
<a name="L271" href="source/drivers/watchdog/it8712f_wdt.c#L271">271</a> 
<a name="L272" href="source/drivers/watchdog/it8712f_wdt.c#L272">272</a>         switch (<a href="ident?i=cmd">cmd</a>) {
<a name="L273" href="source/drivers/watchdog/it8712f_wdt.c#L273">273</a>         case <a href="ident?i=WDIOC_GETSUPPORT">WDIOC_GETSUPPORT</a>:
<a name="L274" href="source/drivers/watchdog/it8712f_wdt.c#L274">274</a>                 if (<a href="ident?i=copy_to_user">copy_to_user</a>(<a href="ident?i=argp">argp</a>, &amp;<a href="ident?i=ident">ident</a>, sizeof(<a href="ident?i=ident">ident</a>)))
<a name="L275" href="source/drivers/watchdog/it8712f_wdt.c#L275">275</a>                         return -<a href="ident?i=EFAULT">EFAULT</a>;
<a name="L276" href="source/drivers/watchdog/it8712f_wdt.c#L276">276</a>                 return 0;
<a name="L277" href="source/drivers/watchdog/it8712f_wdt.c#L277">277</a>         case <a href="ident?i=WDIOC_GETSTATUS">WDIOC_GETSTATUS</a>:
<a name="L278" href="source/drivers/watchdog/it8712f_wdt.c#L278">278</a>                 <a href="ident?i=ret">ret</a> = <a href="ident?i=superio_enter">superio_enter</a>();
<a name="L279" href="source/drivers/watchdog/it8712f_wdt.c#L279">279</a>                 if (<a href="ident?i=ret">ret</a>)
<a name="L280" href="source/drivers/watchdog/it8712f_wdt.c#L280">280</a>                         return <a href="ident?i=ret">ret</a>;
<a name="L281" href="source/drivers/watchdog/it8712f_wdt.c#L281">281</a>                 <a href="ident?i=superio_select">superio_select</a>(<a href="ident?i=LDN_GPIO">LDN_GPIO</a>);
<a name="L282" href="source/drivers/watchdog/it8712f_wdt.c#L282">282</a> 
<a name="L283" href="source/drivers/watchdog/it8712f_wdt.c#L283">283</a>                 <a href="ident?i=value">value</a> = <a href="ident?i=it8712f_wdt_get_status">it8712f_wdt_get_status</a>();
<a name="L284" href="source/drivers/watchdog/it8712f_wdt.c#L284">284</a> 
<a name="L285" href="source/drivers/watchdog/it8712f_wdt.c#L285">285</a>                 <a href="ident?i=superio_exit">superio_exit</a>();
<a name="L286" href="source/drivers/watchdog/it8712f_wdt.c#L286">286</a> 
<a name="L287" href="source/drivers/watchdog/it8712f_wdt.c#L287">287</a>                 return <a href="ident?i=put_user">put_user</a>(<a href="ident?i=value">value</a>, <a href="ident?i=p">p</a>);
<a name="L288" href="source/drivers/watchdog/it8712f_wdt.c#L288">288</a>         case <a href="ident?i=WDIOC_GETBOOTSTATUS">WDIOC_GETBOOTSTATUS</a>:
<a name="L289" href="source/drivers/watchdog/it8712f_wdt.c#L289">289</a>                 return <a href="ident?i=put_user">put_user</a>(0, <a href="ident?i=p">p</a>);
<a name="L290" href="source/drivers/watchdog/it8712f_wdt.c#L290">290</a>         case <a href="ident?i=WDIOC_KEEPALIVE">WDIOC_KEEPALIVE</a>:
<a name="L291" href="source/drivers/watchdog/it8712f_wdt.c#L291">291</a>                 <a href="ident?i=it8712f_wdt_ping">it8712f_wdt_ping</a>();
<a name="L292" href="source/drivers/watchdog/it8712f_wdt.c#L292">292</a>                 return 0;
<a name="L293" href="source/drivers/watchdog/it8712f_wdt.c#L293">293</a>         case <a href="ident?i=WDIOC_SETTIMEOUT">WDIOC_SETTIMEOUT</a>:
<a name="L294" href="source/drivers/watchdog/it8712f_wdt.c#L294">294</a>                 if (<a href="ident?i=get_user">get_user</a>(<a href="ident?i=value">value</a>, <a href="ident?i=p">p</a>))
<a name="L295" href="source/drivers/watchdog/it8712f_wdt.c#L295">295</a>                         return -<a href="ident?i=EFAULT">EFAULT</a>;
<a name="L296" href="source/drivers/watchdog/it8712f_wdt.c#L296">296</a>                 if (<a href="ident?i=value">value</a> &lt; 1)
<a name="L297" href="source/drivers/watchdog/it8712f_wdt.c#L297">297</a>                         return -<a href="ident?i=EINVAL">EINVAL</a>;
<a name="L298" href="source/drivers/watchdog/it8712f_wdt.c#L298">298</a>                 if (<a href="ident?i=value">value</a> &gt; (<a href="ident?i=max_units">max_units</a> * 60))
<a name="L299" href="source/drivers/watchdog/it8712f_wdt.c#L299">299</a>                         return -<a href="ident?i=EINVAL">EINVAL</a>;
<a name="L300" href="source/drivers/watchdog/it8712f_wdt.c#L300">300</a>                 <a href="ident?i=margin">margin</a> = <a href="ident?i=value">value</a>;
<a name="L301" href="source/drivers/watchdog/it8712f_wdt.c#L301">301</a>                 <a href="ident?i=ret">ret</a> = <a href="ident?i=superio_enter">superio_enter</a>();
<a name="L302" href="source/drivers/watchdog/it8712f_wdt.c#L302">302</a>                 if (<a href="ident?i=ret">ret</a>)
<a name="L303" href="source/drivers/watchdog/it8712f_wdt.c#L303">303</a>                         return <a href="ident?i=ret">ret</a>;
<a name="L304" href="source/drivers/watchdog/it8712f_wdt.c#L304">304</a>                 <a href="ident?i=superio_select">superio_select</a>(<a href="ident?i=LDN_GPIO">LDN_GPIO</a>);
<a name="L305" href="source/drivers/watchdog/it8712f_wdt.c#L305">305</a> 
<a name="L306" href="source/drivers/watchdog/it8712f_wdt.c#L306">306</a>                 <a href="ident?i=it8712f_wdt_update_margin">it8712f_wdt_update_margin</a>();
<a name="L307" href="source/drivers/watchdog/it8712f_wdt.c#L307">307</a> 
<a name="L308" href="source/drivers/watchdog/it8712f_wdt.c#L308">308</a>                 <a href="ident?i=superio_exit">superio_exit</a>();
<a name="L309" href="source/drivers/watchdog/it8712f_wdt.c#L309">309</a>                 <a href="ident?i=it8712f_wdt_ping">it8712f_wdt_ping</a>();
<a name="L310" href="source/drivers/watchdog/it8712f_wdt.c#L310">310</a>                 <b><i>/* Fall through */</i></b>
<a name="L311" href="source/drivers/watchdog/it8712f_wdt.c#L311">311</a>         case <a href="ident?i=WDIOC_GETTIMEOUT">WDIOC_GETTIMEOUT</a>:
<a name="L312" href="source/drivers/watchdog/it8712f_wdt.c#L312">312</a>                 if (<a href="ident?i=put_user">put_user</a>(<a href="ident?i=margin">margin</a>, <a href="ident?i=p">p</a>))
<a name="L313" href="source/drivers/watchdog/it8712f_wdt.c#L313">313</a>                         return -<a href="ident?i=EFAULT">EFAULT</a>;
<a name="L314" href="source/drivers/watchdog/it8712f_wdt.c#L314">314</a>                 return 0;
<a name="L315" href="source/drivers/watchdog/it8712f_wdt.c#L315">315</a>         default:
<a name="L316" href="source/drivers/watchdog/it8712f_wdt.c#L316">316</a>                 return -<a href="ident?i=ENOTTY">ENOTTY</a>;
<a name="L317" href="source/drivers/watchdog/it8712f_wdt.c#L317">317</a>         }
<a name="L318" href="source/drivers/watchdog/it8712f_wdt.c#L318">318</a> }
<a name="L319" href="source/drivers/watchdog/it8712f_wdt.c#L319">319</a> 
<a name="L320" href="source/drivers/watchdog/it8712f_wdt.c#L320">320</a> static int <a href="ident?i=it8712f_wdt_open">it8712f_wdt_open</a>(struct <a href="ident?i=inode">inode</a> *<a href="ident?i=inode">inode</a>, struct <a href="ident?i=file">file</a> *<a href="ident?i=file">file</a>)
<a name="L321" href="source/drivers/watchdog/it8712f_wdt.c#L321">321</a> {
<a name="L322" href="source/drivers/watchdog/it8712f_wdt.c#L322">322</a>         int <a href="ident?i=ret">ret</a>;
<a name="L323" href="source/drivers/watchdog/it8712f_wdt.c#L323">323</a>         <b><i>/* only allow one at a time */</i></b>
<a name="L324" href="source/drivers/watchdog/it8712f_wdt.c#L324">324</a>         if (<a href="ident?i=test_and_set_bit">test_and_set_bit</a>(0, &amp;<a href="ident?i=wdt_open">wdt_open</a>))
<a name="L325" href="source/drivers/watchdog/it8712f_wdt.c#L325">325</a>                 return -<a href="ident?i=EBUSY">EBUSY</a>;
<a name="L326" href="source/drivers/watchdog/it8712f_wdt.c#L326">326</a> 
<a name="L327" href="source/drivers/watchdog/it8712f_wdt.c#L327">327</a>         <a href="ident?i=ret">ret</a> = <a href="ident?i=it8712f_wdt_enable">it8712f_wdt_enable</a>();
<a name="L328" href="source/drivers/watchdog/it8712f_wdt.c#L328">328</a>         if (<a href="ident?i=ret">ret</a>)
<a name="L329" href="source/drivers/watchdog/it8712f_wdt.c#L329">329</a>                 return <a href="ident?i=ret">ret</a>;
<a name="L330" href="source/drivers/watchdog/it8712f_wdt.c#L330">330</a>         return <a href="ident?i=nonseekable_open">nonseekable_open</a>(<a href="ident?i=inode">inode</a>, <a href="ident?i=file">file</a>);
<a name="L331" href="source/drivers/watchdog/it8712f_wdt.c#L331">331</a> }
<a name="L332" href="source/drivers/watchdog/it8712f_wdt.c#L332">332</a> 
<a name="L333" href="source/drivers/watchdog/it8712f_wdt.c#L333">333</a> static int <a href="ident?i=it8712f_wdt_release">it8712f_wdt_release</a>(struct <a href="ident?i=inode">inode</a> *<a href="ident?i=inode">inode</a>, struct <a href="ident?i=file">file</a> *<a href="ident?i=file">file</a>)
<a name="L334" href="source/drivers/watchdog/it8712f_wdt.c#L334">334</a> {
<a name="L335" href="source/drivers/watchdog/it8712f_wdt.c#L335">335</a>         if (<a href="ident?i=expect_close">expect_close</a> != 42) {
<a name="L336" href="source/drivers/watchdog/it8712f_wdt.c#L336">336</a>                 <a href="ident?i=pr_warn">pr_warn</a>(<i>"watchdog device closed unexpectedly, will not disable the watchdog timer\n"</i>);
<a name="L337" href="source/drivers/watchdog/it8712f_wdt.c#L337">337</a>         } else if (!<a href="ident?i=nowayout">nowayout</a>) {
<a name="L338" href="source/drivers/watchdog/it8712f_wdt.c#L338">338</a>                 if (<a href="ident?i=it8712f_wdt_disable">it8712f_wdt_disable</a>())
<a name="L339" href="source/drivers/watchdog/it8712f_wdt.c#L339">339</a>                         <a href="ident?i=pr_warn">pr_warn</a>(<i>"Watchdog disable failed\n"</i>);
<a name="L340" href="source/drivers/watchdog/it8712f_wdt.c#L340">340</a>         }
<a name="L341" href="source/drivers/watchdog/it8712f_wdt.c#L341">341</a>         <a href="ident?i=expect_close">expect_close</a> = 0;
<a name="L342" href="source/drivers/watchdog/it8712f_wdt.c#L342">342</a>         <a href="ident?i=clear_bit">clear_bit</a>(0, &amp;<a href="ident?i=wdt_open">wdt_open</a>);
<a name="L343" href="source/drivers/watchdog/it8712f_wdt.c#L343">343</a> 
<a name="L344" href="source/drivers/watchdog/it8712f_wdt.c#L344">344</a>         return 0;
<a name="L345" href="source/drivers/watchdog/it8712f_wdt.c#L345">345</a> }
<a name="L346" href="source/drivers/watchdog/it8712f_wdt.c#L346">346</a> 
<a name="L347" href="source/drivers/watchdog/it8712f_wdt.c#L347">347</a> static const struct <a href="ident?i=file_operations">file_operations</a> <a href="ident?i=it8712f_wdt_fops">it8712f_wdt_fops</a> = {
<a name="L348" href="source/drivers/watchdog/it8712f_wdt.c#L348">348</a>         .<a href="ident?i=owner">owner</a> = <a href="ident?i=THIS_MODULE">THIS_MODULE</a>,
<a name="L349" href="source/drivers/watchdog/it8712f_wdt.c#L349">349</a>         .llseek = <a href="ident?i=no_llseek">no_llseek</a>,
<a name="L350" href="source/drivers/watchdog/it8712f_wdt.c#L350">350</a>         .<a href="ident?i=write">write</a> = <a href="ident?i=it8712f_wdt_write">it8712f_wdt_write</a>,
<a name="L351" href="source/drivers/watchdog/it8712f_wdt.c#L351">351</a>         .unlocked_ioctl = <a href="ident?i=it8712f_wdt_ioctl">it8712f_wdt_ioctl</a>,
<a name="L352" href="source/drivers/watchdog/it8712f_wdt.c#L352">352</a>         .<a href="ident?i=open">open</a> = <a href="ident?i=it8712f_wdt_open">it8712f_wdt_open</a>,
<a name="L353" href="source/drivers/watchdog/it8712f_wdt.c#L353">353</a>         .<a href="ident?i=release">release</a> = <a href="ident?i=it8712f_wdt_release">it8712f_wdt_release</a>,
<a name="L354" href="source/drivers/watchdog/it8712f_wdt.c#L354">354</a> };
<a name="L355" href="source/drivers/watchdog/it8712f_wdt.c#L355">355</a> 
<a name="L356" href="source/drivers/watchdog/it8712f_wdt.c#L356">356</a> static struct <a href="ident?i=miscdevice">miscdevice</a> <a href="ident?i=it8712f_wdt_miscdev">it8712f_wdt_miscdev</a> = {
<a name="L357" href="source/drivers/watchdog/it8712f_wdt.c#L357">357</a>         .<a href="ident?i=minor">minor</a> = <a href="ident?i=WATCHDOG_MINOR">WATCHDOG_MINOR</a>,
<a name="L358" href="source/drivers/watchdog/it8712f_wdt.c#L358">358</a>         .<a href="ident?i=name">name</a> = <i>"watchdog"</i>,
<a name="L359" href="source/drivers/watchdog/it8712f_wdt.c#L359">359</a>         .<a href="ident?i=fops">fops</a> = &amp;<a href="ident?i=it8712f_wdt_fops">it8712f_wdt_fops</a>,
<a name="L360" href="source/drivers/watchdog/it8712f_wdt.c#L360">360</a> };
<a name="L361" href="source/drivers/watchdog/it8712f_wdt.c#L361">361</a> 
<a name="L362" href="source/drivers/watchdog/it8712f_wdt.c#L362">362</a> static int <a href="ident?i=__init">__init</a> <a href="ident?i=it8712f_wdt_find">it8712f_wdt_find</a>(unsigned short *<a href="ident?i=address">address</a>)
<a name="L363" href="source/drivers/watchdog/it8712f_wdt.c#L363">363</a> {
<a name="L364" href="source/drivers/watchdog/it8712f_wdt.c#L364">364</a>         int <a href="ident?i=err">err</a> = -<a href="ident?i=ENODEV">ENODEV</a>;
<a name="L365" href="source/drivers/watchdog/it8712f_wdt.c#L365">365</a>         int <a href="ident?i=chip_type">chip_type</a>;
<a name="L366" href="source/drivers/watchdog/it8712f_wdt.c#L366">366</a>         int <a href="ident?i=ret">ret</a> = <a href="ident?i=superio_enter">superio_enter</a>();
<a name="L367" href="source/drivers/watchdog/it8712f_wdt.c#L367">367</a>         if (<a href="ident?i=ret">ret</a>)
<a name="L368" href="source/drivers/watchdog/it8712f_wdt.c#L368">368</a>                 return <a href="ident?i=ret">ret</a>;
<a name="L369" href="source/drivers/watchdog/it8712f_wdt.c#L369">369</a> 
<a name="L370" href="source/drivers/watchdog/it8712f_wdt.c#L370">370</a>         <a href="ident?i=chip_type">chip_type</a> = <a href="ident?i=superio_inw">superio_inw</a>(<a href="ident?i=DEVID">DEVID</a>);
<a name="L371" href="source/drivers/watchdog/it8712f_wdt.c#L371">371</a>         if (<a href="ident?i=chip_type">chip_type</a> != <a href="ident?i=IT8712F_DEVID">IT8712F_DEVID</a>)
<a name="L372" href="source/drivers/watchdog/it8712f_wdt.c#L372">372</a>                 goto <a href="ident?i=exit">exit</a>;
<a name="L373" href="source/drivers/watchdog/it8712f_wdt.c#L373">373</a> 
<a name="L374" href="source/drivers/watchdog/it8712f_wdt.c#L374">374</a>         <a href="ident?i=superio_select">superio_select</a>(<a href="ident?i=LDN_GAME">LDN_GAME</a>);
<a name="L375" href="source/drivers/watchdog/it8712f_wdt.c#L375">375</a>         <a href="ident?i=superio_outb">superio_outb</a>(1, <a href="ident?i=ACT_REG">ACT_REG</a>);
<a name="L376" href="source/drivers/watchdog/it8712f_wdt.c#L376">376</a>         if (!(<a href="ident?i=superio_inb">superio_inb</a>(<a href="ident?i=ACT_REG">ACT_REG</a>) &amp; 0x01)) {
<a name="L377" href="source/drivers/watchdog/it8712f_wdt.c#L377">377</a>                 <a href="ident?i=pr_err">pr_err</a>(<i>"Device not activated, skipping\n"</i>);
<a name="L378" href="source/drivers/watchdog/it8712f_wdt.c#L378">378</a>                 goto <a href="ident?i=exit">exit</a>;
<a name="L379" href="source/drivers/watchdog/it8712f_wdt.c#L379">379</a>         }
<a name="L380" href="source/drivers/watchdog/it8712f_wdt.c#L380">380</a> 
<a name="L381" href="source/drivers/watchdog/it8712f_wdt.c#L381">381</a>         *<a href="ident?i=address">address</a> = <a href="ident?i=superio_inw">superio_inw</a>(<a href="ident?i=BASE_REG">BASE_REG</a>);
<a name="L382" href="source/drivers/watchdog/it8712f_wdt.c#L382">382</a>         if (*<a href="ident?i=address">address</a> == 0) {
<a name="L383" href="source/drivers/watchdog/it8712f_wdt.c#L383">383</a>                 <a href="ident?i=pr_err">pr_err</a>(<i>"Base address not set, skipping\n"</i>);
<a name="L384" href="source/drivers/watchdog/it8712f_wdt.c#L384">384</a>                 goto <a href="ident?i=exit">exit</a>;
<a name="L385" href="source/drivers/watchdog/it8712f_wdt.c#L385">385</a>         }
<a name="L386" href="source/drivers/watchdog/it8712f_wdt.c#L386">386</a> 
<a name="L387" href="source/drivers/watchdog/it8712f_wdt.c#L387">387</a>         <a href="ident?i=err">err</a> = 0;
<a name="L388" href="source/drivers/watchdog/it8712f_wdt.c#L388">388</a>         <a href="ident?i=revision">revision</a> = <a href="ident?i=superio_inb">superio_inb</a>(<a href="ident?i=DEVREV">DEVREV</a>) &amp; 0x0f;
<a name="L389" href="source/drivers/watchdog/it8712f_wdt.c#L389">389</a> 
<a name="L390" href="source/drivers/watchdog/it8712f_wdt.c#L390">390</a>         <b><i>/* Later revisions have 16-bit values per datasheet 0.9.1 */</i></b>
<a name="L391" href="source/drivers/watchdog/it8712f_wdt.c#L391">391</a>         if (<a href="ident?i=revision">revision</a> &gt;= 0x08)
<a name="L392" href="source/drivers/watchdog/it8712f_wdt.c#L392">392</a>                 <a href="ident?i=max_units">max_units</a> = 65535;
<a name="L393" href="source/drivers/watchdog/it8712f_wdt.c#L393">393</a> 
<a name="L394" href="source/drivers/watchdog/it8712f_wdt.c#L394">394</a>         if (<a href="ident?i=margin">margin</a> &gt; (<a href="ident?i=max_units">max_units</a> * 60))
<a name="L395" href="source/drivers/watchdog/it8712f_wdt.c#L395">395</a>                 <a href="ident?i=margin">margin</a> = (<a href="ident?i=max_units">max_units</a> * 60);
<a name="L396" href="source/drivers/watchdog/it8712f_wdt.c#L396">396</a> 
<a name="L397" href="source/drivers/watchdog/it8712f_wdt.c#L397">397</a>         <a href="ident?i=pr_info">pr_info</a>(<i>"Found IT%04xF chip revision %d - using DogFood address 0x%x\n"</i>,
<a name="L398" href="source/drivers/watchdog/it8712f_wdt.c#L398">398</a>                 <a href="ident?i=chip_type">chip_type</a>, <a href="ident?i=revision">revision</a>, *<a href="ident?i=address">address</a>);
<a name="L399" href="source/drivers/watchdog/it8712f_wdt.c#L399">399</a> 
<a name="L400" href="source/drivers/watchdog/it8712f_wdt.c#L400">400</a> <a href="ident?i=exit">exit</a>:
<a name="L401" href="source/drivers/watchdog/it8712f_wdt.c#L401">401</a>         <a href="ident?i=superio_exit">superio_exit</a>();
<a name="L402" href="source/drivers/watchdog/it8712f_wdt.c#L402">402</a>         return <a href="ident?i=err">err</a>;
<a name="L403" href="source/drivers/watchdog/it8712f_wdt.c#L403">403</a> }
<a name="L404" href="source/drivers/watchdog/it8712f_wdt.c#L404">404</a> 
<a name="L405" href="source/drivers/watchdog/it8712f_wdt.c#L405">405</a> static int <a href="ident?i=__init">__init</a> <a href="ident?i=it8712f_wdt_init">it8712f_wdt_init</a>(void)
<a name="L406" href="source/drivers/watchdog/it8712f_wdt.c#L406">406</a> {
<a name="L407" href="source/drivers/watchdog/it8712f_wdt.c#L407">407</a>         int <a href="ident?i=err">err</a> = 0;
<a name="L408" href="source/drivers/watchdog/it8712f_wdt.c#L408">408</a> 
<a name="L409" href="source/drivers/watchdog/it8712f_wdt.c#L409">409</a>         if (<a href="ident?i=it8712f_wdt_find">it8712f_wdt_find</a>(&amp;<a href="ident?i=address">address</a>))
<a name="L410" href="source/drivers/watchdog/it8712f_wdt.c#L410">410</a>                 return -<a href="ident?i=ENODEV">ENODEV</a>;
<a name="L411" href="source/drivers/watchdog/it8712f_wdt.c#L411">411</a> 
<a name="L412" href="source/drivers/watchdog/it8712f_wdt.c#L412">412</a>         if (!<a href="ident?i=request_region">request_region</a>(<a href="ident?i=address">address</a>, 1, <i>"IT8712F Watchdog"</i>)) {
<a name="L413" href="source/drivers/watchdog/it8712f_wdt.c#L413">413</a>                 <a href="ident?i=pr_warn">pr_warn</a>(<i>"watchdog I/O region busy\n"</i>);
<a name="L414" href="source/drivers/watchdog/it8712f_wdt.c#L414">414</a>                 return -<a href="ident?i=EBUSY">EBUSY</a>;
<a name="L415" href="source/drivers/watchdog/it8712f_wdt.c#L415">415</a>         }
<a name="L416" href="source/drivers/watchdog/it8712f_wdt.c#L416">416</a> 
<a name="L417" href="source/drivers/watchdog/it8712f_wdt.c#L417">417</a>         <a href="ident?i=err">err</a> = <a href="ident?i=it8712f_wdt_disable">it8712f_wdt_disable</a>();
<a name="L418" href="source/drivers/watchdog/it8712f_wdt.c#L418">418</a>         if (<a href="ident?i=err">err</a>) {
<a name="L419" href="source/drivers/watchdog/it8712f_wdt.c#L419">419</a>                 <a href="ident?i=pr_err">pr_err</a>(<i>"unable to disable watchdog timer\n"</i>);
<a name="L420" href="source/drivers/watchdog/it8712f_wdt.c#L420">420</a>                 goto <a href="ident?i=out">out</a>;
<a name="L421" href="source/drivers/watchdog/it8712f_wdt.c#L421">421</a>         }
<a name="L422" href="source/drivers/watchdog/it8712f_wdt.c#L422">422</a> 
<a name="L423" href="source/drivers/watchdog/it8712f_wdt.c#L423">423</a>         <a href="ident?i=err">err</a> = <a href="ident?i=register_reboot_notifier">register_reboot_notifier</a>(&amp;<a href="ident?i=it8712f_wdt_notifier">it8712f_wdt_notifier</a>);
<a name="L424" href="source/drivers/watchdog/it8712f_wdt.c#L424">424</a>         if (<a href="ident?i=err">err</a>) {
<a name="L425" href="source/drivers/watchdog/it8712f_wdt.c#L425">425</a>                 <a href="ident?i=pr_err">pr_err</a>(<i>"unable to register reboot notifier\n"</i>);
<a name="L426" href="source/drivers/watchdog/it8712f_wdt.c#L426">426</a>                 goto <a href="ident?i=out">out</a>;
<a name="L427" href="source/drivers/watchdog/it8712f_wdt.c#L427">427</a>         }
<a name="L428" href="source/drivers/watchdog/it8712f_wdt.c#L428">428</a> 
<a name="L429" href="source/drivers/watchdog/it8712f_wdt.c#L429">429</a>         <a href="ident?i=err">err</a> = <a href="ident?i=misc_register">misc_register</a>(&amp;<a href="ident?i=it8712f_wdt_miscdev">it8712f_wdt_miscdev</a>);
<a name="L430" href="source/drivers/watchdog/it8712f_wdt.c#L430">430</a>         if (<a href="ident?i=err">err</a>) {
<a name="L431" href="source/drivers/watchdog/it8712f_wdt.c#L431">431</a>                 <a href="ident?i=pr_err">pr_err</a>(<i>"cannot register miscdev on minor=%d (err=%d)\n"</i>,
<a name="L432" href="source/drivers/watchdog/it8712f_wdt.c#L432">432</a>                        <a href="ident?i=WATCHDOG_MINOR">WATCHDOG_MINOR</a>, <a href="ident?i=err">err</a>);
<a name="L433" href="source/drivers/watchdog/it8712f_wdt.c#L433">433</a>                 goto reboot_out;
<a name="L434" href="source/drivers/watchdog/it8712f_wdt.c#L434">434</a>         }
<a name="L435" href="source/drivers/watchdog/it8712f_wdt.c#L435">435</a> 
<a name="L436" href="source/drivers/watchdog/it8712f_wdt.c#L436">436</a>         return 0;
<a name="L437" href="source/drivers/watchdog/it8712f_wdt.c#L437">437</a> 
<a name="L438" href="source/drivers/watchdog/it8712f_wdt.c#L438">438</a> 
<a name="L439" href="source/drivers/watchdog/it8712f_wdt.c#L439">439</a> reboot_out:
<a name="L440" href="source/drivers/watchdog/it8712f_wdt.c#L440">440</a>         <a href="ident?i=unregister_reboot_notifier">unregister_reboot_notifier</a>(&amp;<a href="ident?i=it8712f_wdt_notifier">it8712f_wdt_notifier</a>);
<a name="L441" href="source/drivers/watchdog/it8712f_wdt.c#L441">441</a> <a href="ident?i=out">out</a>:
<a name="L442" href="source/drivers/watchdog/it8712f_wdt.c#L442">442</a>         <a href="ident?i=release_region">release_region</a>(<a href="ident?i=address">address</a>, 1);
<a name="L443" href="source/drivers/watchdog/it8712f_wdt.c#L443">443</a>         return <a href="ident?i=err">err</a>;
<a name="L444" href="source/drivers/watchdog/it8712f_wdt.c#L444">444</a> }
<a name="L445" href="source/drivers/watchdog/it8712f_wdt.c#L445">445</a> 
<a name="L446" href="source/drivers/watchdog/it8712f_wdt.c#L446">446</a> static void <a href="ident?i=__exit">__exit</a> <a href="ident?i=it8712f_wdt_exit">it8712f_wdt_exit</a>(void)
<a name="L447" href="source/drivers/watchdog/it8712f_wdt.c#L447">447</a> {
<a name="L448" href="source/drivers/watchdog/it8712f_wdt.c#L448">448</a>         <a href="ident?i=misc_deregister">misc_deregister</a>(&amp;<a href="ident?i=it8712f_wdt_miscdev">it8712f_wdt_miscdev</a>);
<a name="L449" href="source/drivers/watchdog/it8712f_wdt.c#L449">449</a>         <a href="ident?i=unregister_reboot_notifier">unregister_reboot_notifier</a>(&amp;<a href="ident?i=it8712f_wdt_notifier">it8712f_wdt_notifier</a>);
<a name="L450" href="source/drivers/watchdog/it8712f_wdt.c#L450">450</a>         <a href="ident?i=release_region">release_region</a>(<a href="ident?i=address">address</a>, 1);
<a name="L451" href="source/drivers/watchdog/it8712f_wdt.c#L451">451</a> }
<a name="L452" href="source/drivers/watchdog/it8712f_wdt.c#L452">452</a> 
<a name="L453" href="source/drivers/watchdog/it8712f_wdt.c#L453">453</a> <a href="ident?i=module_init">module_init</a>(<a href="ident?i=it8712f_wdt_init">it8712f_wdt_init</a>);
<a name="L454" href="source/drivers/watchdog/it8712f_wdt.c#L454">454</a> <a href="ident?i=module_exit">module_exit</a>(<a href="ident?i=it8712f_wdt_exit">it8712f_wdt_exit</a>);
<a name="L455" href="source/drivers/watchdog/it8712f_wdt.c#L455">455</a> </pre></div><p>
This page was automatically generated by <a href="https://sourceforge.net/projects/lxr/">LXR</a> 0.3.1 (<a href="http://free-electrons.com/pub/source/lxr-0.3.1-fe1.tar.bz2">source</a>).
&nbsp;&bull;&nbsp;
Linux is a registered trademark of Linus Torvalds
&nbsp;&bull;&nbsp;
<a href="http://free-electrons.com/company/contact/">Contact us</a>
</p>

<div id="menu">
<ul>
   <li><a href="http://free-electrons.com/">Home</a></li>
   <li><a href="http://free-electrons.com/development/" title="Development">Development</a></li>
   <li><a href="http://free-electrons.com/services/" title="Services">Services</a></li>
   <li><a href="http://free-electrons.com/training/" title="Training">Training</a></li>
   <li><a href="http://free-electrons.com/docs/" title="Docs">Docs</a></li>
   <li><a href="http://free-electrons.com/community/" title="Community">Community</a></li>
   <li><a href="http://free-electrons.com/company/" title="Company">Company</a></li>
   <li><a href="http://free-electrons.com/blog/" title="Blog">Blog</a></li>
</ul>
</div>

</div>
</div>
</body>
</html>
