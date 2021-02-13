# CGA Animation Skinning

Using conformal geometric algebra (cga) for mesh skinning / vertex blending / skeletal animation. This technique is known as Dual Quaternion Skinning (DQS).

## References on Skinning

Only realtime solution are listed.

### Linear Blend Skinning (LBS)

The most popular implementation but there are artifacts such as twisting motion.

| Name | Year | Type | Description |
| --- | --- | --- | --- |
| [Skeletal Animation with Assimp](http://ogldev.atspace.co.uk/www/tutorial38/tutorial38.html) | | Website | |
| [WebGL Skinning](https://webglfundamentals.org/webgl/lessons/webgl-skinning.html) | | Website | |
| [WebGL 2 Skinning](https://webgl2fundamentals.org/webgl/lessons/webgl-skinning.html) | | Website | |
| [GLTF Skinning](https://github.com/KhronosGroup/glTF-Tutorials/blob/master/gltfTutorial/gltfTutorial_020_Skins.md) | | Website | |
| [Skinned models in DirectX 11](http://www.richardssoftware.net/2013/10/skinned-models-in-directx-11-with.html) | | Website | |
| [Handmade Hero Chat 19 - Introduction to Mesh Skinning](https://www.youtube.com/watch?v=sd-d4Z7utVM) | 2019 | Video | By [Casey Muratori](https://handmade.network/m/cmuratori) |

### Pose Space Deformation (PSD)

A data-driven method which compute the deformation as a combination of basic poses. Mostly used for faces and regrouped in the Morphing category.

| Name | Year | Type | Description |
| --- | --- | --- | --- |
| [Pose Space Deformation: A Unified Approach to Shape Interpolation and Skeleton-Driven Deformation (PSD)](http://scribblethink.org/Work/PSD/PSD.pdf) | 2000 | Paper | Explain drawbacks of vertex blending (without quaternions) |
| [GDC 2015 - Real-time transformations in the order 1886](https://readyatdawn.sharefile.com/share/view/s892ebe64d544726b) | 2015 | Slide | |

### Physics Based Skinning (PBS)

Great but heavy on performance side, in games used in conjonction with other systems

| Name | Year | Type | Description |
| --- | --- | --- | --- |
| [GDC 2017 - Physics Animation in Uncharted 4: A Thief's End](https://www.youtube.com/watch?v=7S-_vuoKgR4) | 2017 | Video | |

### **Dual Quaternion Skinning (DQS)**

There can be a Bulging effect, but it can be partly removed. Supposed to be 1.5 times less performant in computation time than LBS.

| Name | Year | Type | Description |
| --- | --- | --- | --- |
| [Skinning with Dual Quaternions](https://team.inria.fr/imagine/files/2014/10/skinning_dual_quaternions.pdf) | 2007 | Paper | |
| [Geometric skinning with approximate dual quaternion blending](https://www.cs.utah.edu/~ladislav/kavan08geometric/kavan08geometric.html) | 2008 | Paper | Follow-up Paper to Skinning with Dual Quaternions by the same team |
| [Bulging-free dual quaternion skinning](https://sci-hub.se/https://onlinelibrary.wiley.com/doi/abs/10.1002/cav.1604) | 2014 | Paper | |
| [Dual Quaternions skinning tutorial and C++ codes](http://rodolphe-vaillant.fr/?e=29) | | Website | |
| [Dual Quaternion Shader Explained line by line](https://www.chinedufn.com/dual-quaternion-shader-explained/) | 2017 | Website | |
| [Fun with WebGL 2.0 - Dual Quaternions with Skinning](https://www.youtube.com/watch?v=pUeBOymcEw0) | 2017 | Video | By SketchpunkLabs |

### Center of Rotation (CoR)

Great but seems less performant than DQS and needs pre-processing and more data added to the pipeline.

| Name | Year | Type | Description |
| --- | --- | --- | --- |
| [Real-time skeletal skinning with optimized centers of rotation](https://la.disneyresearch.com/publication/skinning-with-optimized-cors/) | 2016 | Paper | Disney research |
| [CoR in Unity](https://forum.unity.com/threads/cor-real-time-skeletal-skinning-with-optimized-centers-of-rotation.634435/) | 2016 | Forum post | Open source implementation in Unity of the CoR Paper |
___

## General References

### Papers

| Name | Year | Description |
| --- | --- | --- |
| [An inclusive Conformal Geometric Algebra GPU animation interpolation and deformation algorithm](http://www.gaalop.de/wp-content/uploads/CGI_CGA_Paper.pdf) | 2016 | |
| [Deform, Cut and Tear a skinned model using Conformal Geometric Algebra](https://arxiv.org/pdf/2007.04464v2.pdf) | 2020 | |
| [Geometric algebra rotorns for skinned character animation blending](http://george.papagiannakis.org/wp-content/uploads/2013/09/GArotorsForSkinnedCharacterAnimationBlending-1.5.pdf) | 2013 | |
| [Geometric Computing in Computer Graphics and Robotics using Conformal Geometric Algebra](http://tuprints.ulb.tu-darmstadt.de/764/1/DissertationDH061213.pdf) | 2006 | |
| [Improved Vertex Skinning Algorithm based on Dual Quaternions](https://ir.canterbury.ac.nz/bitstream/handle/10092/16776/Yin,%20Hao_Msc%20Thesis.pdf) | 2019 | University of Canterbury |
| [A beginners guide to Dual-Quaternions](https://cs.gmu.edu/~jmlien/teaching/cs451/uploads/Main/dual-quaternion.pdf) | 2012 | |
| [SIGGRAPH 2019 - Geometric Algebra for Computer Graphics - Course notes](https://arxiv.org/pdf/2002.04509.pdf) | 2019 | |

### Repositories

| Name | Description |
| --- | --- |
| [GA Bounce](https://github.com/qcoumes/ga-bounce) | |
| [GA Raytracer](https://github.com/torresf/ga-raytracer) | |
| [GA Shaders](https://github.com/dragonbleapiece/GAShaders) | |

### Slides

| Name | Description |
| --- | --- |
| [GDC 2015 - Maths for game programmers : Inverse kinematics revisited](http://www.dtecta.com/files/GDC15_VanDenBergen_Gino_Math_Tut.pdf) | By Gino van den Bergen |

### Videos

| Name | Description |
| --- | --- |
| [GDC 2013 - Maths for game programmers : Understanding Quaternions](https://www.gdcvault.com/play/1017653/Math-for-Game-Programmers-Understanding) | |
| [Dual Quaternion Demystified](https://www.youtube.com/watch?v=ichOiuBoBoQ) | GAME2020 talk by Steven De Keninck |
| [3Blue1Brown - Quaternions and 3d rotation](https://www.youtube.com/watch?v=zjMuIxRvygQ) | |
| [Siggraph 2019 - Geometric Algebra](https://www.youtube.com/watch?v=tX4H_ctggYo) | By [Bivector](https://bivector.net/) |
| [Understanding Quaternions through Geometric Algebra](https://www.youtube.com/watch?v=eo2HNCTV78s) | By [Jorge Rodriguez](https://www.youtube.com/channel/UCEhBM2x5MG9-e_JSOzU068w) |

### Websites

| Name | Description |
| --- | --- |
| [Maths - Dual Quaternions](https://www.euclideanspace.com/maths/algebra/realNormedAlgebra/other/dualQuaternion/index.htm) | |
| [Projective Geometric Algebra done right](http://terathon.com/blog/projective-geometric-algebra-done-right/) | Blog post by [Eric Lengyel](http://www.terathon.com/lengyel/) in 2020 |
| [Let's remove Quaternions from every 3D engine](https://marctenbosch.com/quaternions/) | Interactive Introduction to Rotors from Geometric Algebra by [Marc ten Bosch](https://marctenbosch.com/) |

### Books

| Name                                                         | Description                                                  | Illustration |
| ------------------------------------------------------------ | ------------------------------------------------------------ | ------------ |
| [Game Engine Architecture](https://www.gameenginebook.com/)  | A famous book by Jason Gregory, a Naughty Dog engineer. Check chap.12 about Animation Systems       | <img width="80" src="https://www.amazon.fr/images/I/41Hz1rTfm4L._SX260_.jpg">            |
| [3D Math Primer for Graphics and Game Development, 2nd Edition](https://www.crcpress.com/3D-Math-Primer-for-Graphics-and-Game-Development/Dunn/p/book/9781568817231) | Fletcher Dunn. Skinning is presented p. 424 | <img width="80" src="https://images.tandf.co.uk/common/jackets/amazon/978156881/9781568817231.jpg"> |
| [Real Time Rendering](http://www.realtimerendering.com/book.html) | Famous book on rendering techniques. Check p.84 about vertex blending. About dual quaternions in skinning "Computation is less than 1.5x the cost for linear skin blending and the results are good, which has led to rapid adoption of this technique"                          | <img width="80" src="https://images-na.ssl-images-amazon.com/images/I/81E9-e9Ek+L.jpg">       
| [GPU Pro 5 - Quaternions Revisited](http://gpupro.blogspot.com/search/label/Skinning) | An 2014 Article by Peter Sikachev, Sergey Makeev and Vladimir Egorov. Sample code can be found [here](https://github.com/SergeyMakeev/Quaternions-Revisited) | <img width="80" src="https://images-na.ssl-images-amazon.com/images/I/51ew7AfD8SL._SX258_BO1,204,203,200_.jpg"> |
| [Geometric Algebra For Computer Science](https://geometricalgebra.org/) | | <img width="80" src="https://geometricalgebra.org/images/cover.png"> |
| [Geometric Algebra For Computer Graphics](https://www.springer.com/gp/book/9781846289965) | | <img width="80" src="https://images-na.ssl-images-amazon.com/images/I/51bI7NldFnL.jpg"> |
