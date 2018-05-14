﻿using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using template;

namespace Template
{
	public class OpenTKApp : GameWindow
	{
		static int screenID, screenIDDebug;
        public int screenSize = 512;
        public int screenDebugSize = 1024;
		static Raytracer raytracer;
		static bool terminated = false;
		protected override void OnLoad( EventArgs e )
		{
			// called upon app init
			GL.ClearColor( Color.Black );
			GL.Enable( EnableCap.Texture2D );
			GL.Disable( EnableCap.DepthTest );
			GL.Hint( HintTarget.PerspectiveCorrectionHint, HintMode.Nicest );
			ClientSize = new Size(1024, 512);
            raytracer = new Raytracer
            {
                scene = new Scene(),
                camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1)),
                screen = new Surface(screenSize, screenSize, new Vector3(-5, 5, 2), new Vector3(5, 5, 2), new Vector3(5,-5,2)),
                screenDebug = new Surface(screenDebugSize, screenDebugSize, Vector3.Zero, Vector3.Zero, Vector3.Zero)
            };
            Sprite.target = raytracer.screen;
			screenID = raytracer.screen.GenTexture();
            screenIDDebug = raytracer.screenDebug.GenTexture();
            raytracer.Init();
		}
		protected override void OnUnload( EventArgs e )
		{
			// called upon app close
			GL.DeleteTextures( 1, ref screenID );
			Environment.Exit( 0 ); // bypass wait for key on CTRL-F5
		}
		protected override void OnResize( EventArgs e )
		{
			// called upon window resize
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			GL.Ortho( -1.0, 1.0, -1.0, 1.0, 0.0, 4.0 );
		}
		protected override void OnUpdateFrame( FrameEventArgs e )
		{
			// called once per frame; app logic
			var keyboard = OpenTK.Input.Keyboard.GetState();
			if (keyboard[OpenTK.Input.Key.Escape]) this.Exit();
		}
		protected override void OnRenderFrame( FrameEventArgs e )
		{
            // called once per frame; render
            raytracer.Render();
			if (terminated) 
			{
				Exit();
				return;
			}
			// convert Game.screen to OpenGL texture
			GL.BindTexture( TextureTarget.Texture2D, screenID );
            GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, raytracer.screen.width, raytracer.screen.height, 0, 
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, raytracer.screen.pixels);
            // clear window contents
            GL.Clear( ClearBufferMask.ColorBufferBit );
			// setup camera
			GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			// draw screen filling quad 
			GL.Begin( PrimitiveType.Quads );
            
			GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( -1.0f, -1.0f );
			GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2(  0f, -1.0f );
			GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2(  0f,  1.0f );
			GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( -1.0f,  1.0f );
            GL.End();
            
            
            // convert Game.screen to OpenGL texture
            GL.BindTexture(TextureTarget.Texture2D, screenIDDebug);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, raytracer.screenDebug.width, raytracer.screenDebug.height, 0,
                           OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, raytracer.screenDebug.pixels);
            // clear window contents
            // setup camera
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            // draw screen filling quad

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(0f, 1.0f);
            GL.End();
            
            
            // tell OpenTK we're done rendering
            SwapBuffers();
		}
		public static void Main( string[] args ) 
		{ 
			// entry point
			using (OpenTKApp app = new OpenTKApp()) { app.Run( 30.0, 0.0 ); }
		}
	}
}