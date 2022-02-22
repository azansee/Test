﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Uow
{
    public class UnitOfWork_Events_Tests : AbpIntegratedTest<AbpDddModule>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWork_Events_Tests()
        {
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public void Should_Trigger_Complete_On_Success()
        {
            var completed = false;
            var disposed = false;

            using (var uow = _unitOfWorkManager.Begin())
            {
                uow.Completed += (sender, args) => completed = true;
                uow.Disposed += (sender, args) => disposed = true;

                uow.Complete();

                completed.ShouldBeTrue();
            }

            disposed.ShouldBeTrue();
        }

        [Fact]
        public void Should_Trigger_Complete_On_Success_In_Child_Uow()
        {
            var completed = false;
            var disposed = false;

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (var childUow = _unitOfWorkManager.Begin())
                {
                    childUow.Completed += (sender, args) => completed = true;
                    uow.Disposed += (sender, args) => disposed = true;

                    childUow.Complete();

                    completed.ShouldBeFalse(); //Parent has not been completed yet!
                    disposed.ShouldBeFalse();
                }

                completed.ShouldBeFalse(); //Parent has not been completed yet!
                disposed.ShouldBeFalse();

                uow.Complete();

                completed.ShouldBeTrue(); //It's completed now!
                disposed.ShouldBeFalse(); //But not disposed yet!
            }

            disposed.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Trigger_Complete_If_Uow_Is_Not_Completed()
        {
            var completed = false;
            var failed = false;
            var disposed = false;

            using (var uow = _unitOfWorkManager.Begin())
            {
                uow.Completed += (sender, args) => completed = true;
                uow.Failed += (sender, args) => failed = true;
                uow.Disposed += (sender, args) => disposed = true;
            }

            completed.ShouldBeFalse();
            failed.ShouldBeTrue();
            disposed.ShouldBeTrue();
        }

        [Fact]
        public void Should_Trigger_Failed_If_Uow_Throws_Exception()
        {
            var completed = false;
            var failed = false;
            var disposed = false;

            Assert.Throws<Exception>(new Action(() =>
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    uow.Completed += (sender, args) => completed = true;
                    uow.Failed += (sender, args) => failed = true;
                    uow.Disposed += (sender, args) => disposed = true;

                    throw new Exception("test exception");
                }
            })).Message.ShouldBe("test exception");

            completed.ShouldBeFalse();
            failed.ShouldBeTrue();
            disposed.ShouldBeTrue();
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void Should_Trigger_Failed_If_Rolled_Back(bool callComplete)
        {
            var completed = false;
            var failed = false;
            var disposed = false;

            using (var uow = _unitOfWorkManager.Begin())
            {
                uow.Completed += (sender, args) => completed = true;
                uow.Failed += (sender, args) => { failed = true; args.IsRolledback.ShouldBeTrue(); };
                uow.Disposed += (sender, args) => disposed = true;

                uow.Rollback();

                if (callComplete)
                {
                    uow.Complete();
                }
            }

            completed.ShouldBeFalse();
            failed.ShouldBeTrue();
            disposed.ShouldBeTrue();
        }
    }
}
