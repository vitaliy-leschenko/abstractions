﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace Unity
{
    /// <summary>
    /// Interface defining the behavior of the Unity dependency injection container.
    /// </summary>
    public interface IUnityContainer : IDisposable
    {
        /// <summary>
        /// Register a type mapping with the container, where the created instances will use
        /// the given <see cref="LifetimeManager"/>.
        /// </summary>
        /// <param name="typeFrom"><see cref="Type"/> that will be requested.</param>
        /// <param name="typeTo"><see cref="Type"/> that will actually be returned.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetimeManager">The <see cref="LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects. Can be null.</param>
        /// <returns>The <see cref="IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        IUnityContainer RegisterType(Type typeFrom, Type typeTo, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers);

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like setting a type as a singleton, except that instead
        /// of the container creating the instance the first time it is requested, the user
        /// creates the instance ahead of type and adds that instance to the container.
        /// </para>
        /// </remarks>
        /// <param name="type">Type of instance to register (may be an implemented interface instead of the full type).</param>
        /// <param name="instance">Object to returned.</param>
        /// <param name="name">Name for registration.</param>
        /// <param name="lifetime">
        /// <see cref="LifetimeManager"/> object that controls how this instance will be managed by the container.</param>
        /// <returns>The <see cref="IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        IUnityContainer RegisterInstance(Type type, string name, object instance, LifetimeManager lifetime);

        /// <summary>
        /// Resolve an instance of the requested type with the given name typeFrom the container.
        /// </summary>
        /// <param name="type"><see cref="Type"/> of object to get typeFrom the container.</param>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <param name="resolverOverrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object.</returns>
        object Resolve(Type type, string name, params ResolverOverride[] resolverOverrides);

        /// <summary>
        /// Run an existing object through the container and perform injection on it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful when you don'type control the construction of an
        /// instance (ASP.NET pages or objects created via XAML, for instance)
        /// but you still want properties and other injection performed.
        /// </para></remarks>
        /// <param name="type"><see cref="Type"/> of object to perform injection on.</param>
        /// <param name="existing">Instance to build up.</param>
        /// <param name="name">name to use when looking up the TypeMappings and other configurations.</param>
        /// <param name="resolverOverrides">Any overrides for the resolve calls.</param>
        /// <returns>The resulting object. By default, this will be <paramref name="existing"/>, but
        /// container extensions may add things like automatic proxy creation which would
        /// cause this to return a different object (but still type compatible with <paramref name="type"/>).</returns>
        object BuildUp(Type type, object existing, string name, params ResolverOverride[] resolverOverrides);

        /// <summary>
        /// Add an extension object to the container.
        /// </summary>
        /// <param name="extension"><see cref="UnityContainerExtension"/> to add.</param>
        /// <returns>The <see cref="IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        IUnityContainer AddExtension(UnityContainerExtension extension);

        /// <summary>
        /// Resolve access to a configuration interface exposed by an extension.
        /// </summary>
        /// <remarks>Extensions can expose configuration interfaces as well as adding
        /// strategies and policies to the container. This method walks the list of
        /// added extensions and returns the first one that implements the requested type.
        /// </remarks>
        /// <param name="configurationInterface"><see cref="Type"/> of configuration interface required.</param>
        /// <returns>The requested extension's configuration interface, or null if not found.</returns>
        object Configure(Type configurationInterface);

        /// <summary>
        /// Remove all installed extensions typeFrom this container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method removes all extensions typeFrom the container, including the default ones
        /// that implement the out-of-the-box behavior. After this method, if you want to use
        /// the container again you will need to either read the default extensions or replace
        /// them with your own.
        /// </para>
        /// <para>
        /// The registered instances and singletons that have already been set up in this container
        /// do not get removed.
        /// </para>
        /// </remarks>
        /// <returns>The <see cref="IUnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).</returns>
        IUnityContainer RemoveAllExtensions();

        /// <summary>
        /// The parent of this container.
        /// </summary>
        /// <value>The parent container, or null if this container doesn'type have one.</value>
        IUnityContainer Parent { get; }

        /// <summary>
        /// Create a child container.
        /// </summary>
        /// <remarks>
        /// A child container shares the parent's configuration, but can be configured with different
        /// settings or lifetime.</remarks>
        /// <returns>The new child container.</returns>
        IUnityContainer CreateChildContainer();

        // TODO: Add summary
        bool IsRegistered(Type type, string name);

        /// <summary>
        /// GetOrDefault a sequence of <see cref="IContainerRegistration"/> that describe the current state
        /// of the container.
        /// </summary>
        IEnumerable<IContainerRegistration> Registrations { get; }
    }
}
