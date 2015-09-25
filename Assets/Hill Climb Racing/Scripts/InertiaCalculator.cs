
using UnityEngine;
using System.Collections;

public class InertiaCalculator : MonoBehaviour
{
	public class MassData
	{
		public float mass, inertia;
		public Vector2 center;
	}
	
	public float inertia = 1f;
	public float multiplier = 1f;
	
	void OnEnable()
	{
		if(rigidbody2D != null)
		{
			//CALCULATE
			//You can set it up to be automatic, but this whole thing is kinda slow, so better pre-calculate inertia in editor.
			//inertia = ComputeInertia(rigidbody2D);
			
			//SET
			rigidbody2D.inertia = inertia * multiplier;
		}
	}
	
	static public float ComputeInertia(Rigidbody2D body)
	{
		//CALCULATE INERTIA, TAKING SCALING AND ALL THE CHILD COLLIDERS INTO ACCOUNT
		float hypothetic_mass = 0f;
		float hypothetic_density = 1f;
		float I = 0f;
		Vector2 center = Vector2.zero;
		
		Collider2D[] colliders = body.GetComponentsInChildren<Collider2D>();
		
		for(int i = 0;i < colliders.Length; ++i)
		{
			Collider2D c = colliders[i];
			//PICK ONLY OWN COMPS AND ADDITIONAL CHILD COLLIDERS THAT USE SAME BODY
			if(c.gameObject == body.gameObject || (c.transform.parent.gameObject == body.gameObject && c.rigidbody2D == null))
			{
				MassData data = null;
				
				CircleCollider2D circle = c as CircleCollider2D;
				if(circle != null) data = CircleMassInertiaCenter(circle, hypothetic_density, body.transform.position);
				
				BoxCollider2D box = c as BoxCollider2D;
				if(box != null) data = BoxMassInertiaCenter(box, hypothetic_density, body.transform.position);
				
				PolygonCollider2D poly = c as PolygonCollider2D;
				if(poly != null) data = PolygonMassInertiaCenter(poly, hypothetic_density, body.transform.position);
				
				if(data != null)
				{
					hypothetic_mass += data.mass;
					center += data.center * data.mass;
					I += data.inertia;
				}
			}
		}
		
		//PORTED FROM BOX2D
		// Compute the center of mass.
		center *= 1f / hypothetic_mass;
		
		// Center the inertia about the center of mass
		I -= hypothetic_mass * center.sqrMagnitude;
		//SCALE IT
		I *= body.mass / hypothetic_mass;
		
		return I;
	}
	
	static public MassData CircleMassInertiaCenter(CircleCollider2D collider, float density, Vector2 center)
	{
		//PORTED FROM BOX2D
		float r = collider.radius * collider.transform.lossyScale.x;
		float m = density * Mathf.PI * r * r;
		center = (Vector2)collider.transform.TransformPoint(collider.center) - center;
		float I = m * (0.5f * r * r + center.sqrMagnitude);
		MassData data = new MassData();
		data.inertia = I;
		data.mass = m;
		data.center = center;
		return data;
	}
	static public MassData BoxMassInertiaCenter(BoxCollider2D collider, float density, Vector2 center)
	{
		Vector2[] points = new Vector2[4];
		points[3] = -collider.size * 0.5f + collider.center;
		points[2] = new Vector2(-collider.size.x, collider.size.y) * 0.5f + collider.center;
		points[1] = collider.size * 0.5f + collider.center;
		points[0] = new Vector2(collider.size.x, -collider.size.y) * 0.5f + collider.center;
		
		for(int i = 0;i < points.Length; ++i) points[i] = (Vector2)collider.transform.TransformPoint(points[i]) - center;
		return PolygonMassInertiaCenter(points, density);
	}
	
	static public MassData PolygonMassInertiaCenter(PolygonCollider2D collider, float density, Vector2 center)
	{
		Vector2[] points = collider.points;
		for(int i = 0;i < points.Length; ++i) points[i] = (Vector2)collider.transform.TransformPoint(points[i]) - center;
		return PolygonMassInertiaCenter(points, density);
	}
	
	static public MassData PolygonMassInertiaCenter(Vector2[] points, float density)
	{
		//PORTED FROM BOX2D
		Vector2 center = Vector2.zero;
		float area = 0f;
		float I = 0f;
		
		// pRef is the reference point for forming triangles.
		// It's location doesn't change the result (except for rounding error).
		Vector2 p1 = Vector2.zero;
		
		float k_inv3 = 1f / 3f;
		
		
		for (int i = 0; i < points.Length; ++i)
		{
			// Triangle vertices.
			//b2Vec2 p1 = pRef;
			//
			//b2Vec2 p2 = m_vertices[i];
			Vector2 p2 = points[i];
			//b2Vec2 p3 = i + 1 < m_vertexCount ? m_vertices[i+1] : m_vertices[0];
			Vector2 p3 = i + 1 < points.Length ? points[i + 1] : points[0];
			
			//b2Vec2 e1 = p2 - p1;
			Vector2 e1 = p2 - p1;
			//b2Vec2 e2 = p3 - p1;
			Vector2 e2 = p3 - p1;
			
			//float32 D = b2Cross(e1, e2);
			float D = e1.x * e2.y - e1.y * e2.x;
			
			//float32 triangleArea = 0.5f * D;
			float triangleArea = 0.5f * D;
			area += triangleArea;
			
			// Area weighted centroid
			//center += triangleArea * k_inv3 * (p1 + p2 + p3);
			center += triangleArea * k_inv3 * (p1 + p2 + p3);
			
			//float32 px = p1.x, py = p1.y;
			float px = p1.x;
			float py = p1.y;
			//float32 ex1 = e1.x, ey1 = e1.y;
			float ex1 = e1.x;
			float ey1 = e1.y;
			//float32 ex2 = e2.x, ey2 = e2.y;
			float ex2 = e2.x;
			float ey2 = e2.y;
			
			//MAGICS
			//float32 intx2 = k_inv3 * (0.25f * (ex1*ex1 + ex2*ex1 + ex2*ex2) + (px*ex1 + px*ex2)) + 0.5f*px*px;
			float intx2 = k_inv3 * (0.25f * (ex1 * ex1 + ex2 * ex1 + ex2 * ex2) + (px * ex1 + px*ex2)) + 0.5f * px * px;
			//float32 inty2 = k_inv3 * (0.25f * (ey1*ey1 + ey2*ey1 + ey2*ey2) + (py*ey1 + py*ey2)) + 0.5f*py*py;
			float inty2 = k_inv3 * (0.25f * (ey1 * ey1 + ey2 * ey1 + ey2 * ey2) + (py * ey1 + py * ey2)) + 0.5f * py * py;
			
			I += D * (intx2 + inty2);
		}
		
		// Total mass
		float m = density * area;
		
		// Center of mass
		//b2Settings.b2Assert(area > Number.MIN_VALUE);
		//center *= 1.0f / area;
		center *= 1f / area;
		
		// Inertia tensor relative to the local origin.
		I = density * I;
		
		MassData data = new MassData();
		data.inertia = I;
		data.mass = m;
		data.center = center;
		
		return data;
	}
}

